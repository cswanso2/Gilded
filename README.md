To start off I set out to create the endpoints required in the doc. I also created four extra endpoints to support them. IE registration and user balance. In addition item creation.

I'll start off with the questions form the readme.
	1. I chose to go with a Json data format. Pretty standard stuff, universal easy to read. I also made the  uri's in a restful manner. Including details about the object in the uri
	where applicable. This request is requried For instance to create an item the request is
		POST "http://localhost:56882/api/items"
		Headers
		Content-Type: application/json
		Authorization: adminapikey
		Body:
		{
			"name": "testitem",
			"price": 5,
			"description": "bla"
		}.
		RESPONSE: 201. 409 on duplicate 
	Note, to see items in a list they must have an inventory. By default items posted to the server have 
	no inventory and it can be increase by hitting 
		PUT: "http://localhost:56882/api/items/{itemName}/inventories/{amountChanged}
		Headers: Content-Type: application/json
		Response: 202, 404 on missing item
	To retrieve the list of items hit
		GET "http://localhost:56882/api/items"
		Headers
		Content-Type: application/json
		Response: 200
		[
			  {
			    "Name": "testitem",
			    "Description": "description",
			    "Price": 5
			  },
			  {
			    "Name": "testitem2",
			    "Description": "description",
			    "Price": 5
			  },
			  {
			    "Name": "testitem3",
			    "Description": "description",
			    "Price": 6
			  }
		]

	2. I waffled hard on authentication mechanism between usernames and passwords and apikeys. Ultimately this article https://stormpath.com/blog/top-six-reasons-use-api-keys-and-how, 
	and being super wierded out by no database passwords drove me to apikeys. There is two classes of users, "Admin" and "User". Admins can create items and update inventories. Users can purchase items, and add balance to their accounts. I do instantiate a admin user on startup with an apikey of "adminapikey". Obviously not secure, but needed a admin user.

	The other questions to consider:
		1. I require users to pass their api keys in the Authorization header before making authorized requests, such as purchasing or increasing their balance. 
		I then wrap authorized controller methods in a filter. This filter calls the user repository, which substitutes for a database, and validates that it is a real user and they have
		permission for the action.
		2. Is it always possible to buy an item, I could think of four cases where it isn't:
			a. User isn't authenticated
			b. User has less "credits" than an items price
			c. Item isn't in stock. 
			d. Weird concurrency issues.
			I handled user authenticating error with the filter. Less credits, just check how many a user has when purchasing a item. Item isn't in stock don't allow the item to be purchased. Weird concurrency issues is kind a of a catch all for where two simultaneous actions could be impossible. I.E two users try to purchase an item with one in stock at the same time. Or a user with 4 credits trys to buy an item worth 3 credits and 2 credits simultaneously. I did take an easy way out, on the purchase manager I made a static lock so only one person is allowed to purchase an item at a time. This solution would obviously scale horribly, would consider using some sort of producer consumer pattern if this became a breaking points of the application. 

	In place of the database I made singleton repositories of Items, and Users. They are accessed by calling Get() on them. I aimed pretty high for unit test coverage, I know I missed the filter, will admit to that now. I wrote integration tests for all of the endpoints, attempting to try to hit conceivable pattern flows. To manage the purchasing of items I made a data orchestrator type class. PurchaseManager. This interacts with both repositories and decrements a users  funds as well as the item count. I made some custom exceptions too 
	where no usual exceptions were applicable. I tried to handle all expected corruptions in data as such to not leak 500s to the end users. The conrollers do interact with the datalayer 
	directly sometimes. I didn't see the need to move through a dataorchestrator since the repositories have more logic than a normal datalayer. Finally here's reference for all endpoints 
	feel free to skip this:

	1. Create items
		POST "http://localhost:56882/api/items"
		Headers:
			Content-Type: application/json
			Authorization: adminapikey 
		Body:
			{
				"name": "testitem",
				"price": 5,
				"description": "bla"
			}.
		RESPONSE: 201. 409 on duplicate 
		Notes: Creates an item. Will not create duplicates, authorization is required to be an administrator. The item will have zero inventory
	2. Get Items
		GET "http://localhost:56882/api/items"
		Headers:
			Content-Type: application/json
		Response: 200
		[
			  {
			    "Name": "testitem",
			    "Description": "description",
			    "Price": 5
			  },
			  {
			    "Name": "testitem2",
			    "Description": "description",
			    "Price": 5
			  },
			  {
			    "Name": "testitem3",
			    "Description": "description",
			    "Price": 6
			  }
		]
		Notes: Should always be a 200, will return empty list if there is no items. To show up in a list an item must have an inventory greater than zero.

		3. Update inventory
		PUT "http://localhost:56882/api/items/{itemName}/inventories/{numberOfNewItems}"
		Headers:
			Content-Type: application/json
			Authorization: adminapikey 
		Body:
		{
		}
		Content-Type: application/json
		Response: 202, 404 if item doesn't exist
		Notes: Increase the inventory if an item by numberofnewitems for instance if an item has five inventory and number of newitems is 6 it will go to 11

		4. Purchase item
		Post "http://localhost:56882/api/items/{itemName}/purchases"
		Headers:
			Content-Type: application/json
			Authorization: {yourapikey} 
		Body:
		{
		}
		Content-Type: application/json
		Response: 202, 402 (Payment required) if insufficient credits 404 if item doesn't exist
		Notes: Will purchase an item. I don't track purchases anywhere. In a real application they would assumably be stored in a database. Use is determind by auth header.
 
		5. Register
		Post "http://localhost:56882/api/users"
		Headers:
			Content-Type: application/json
		Body:
		{
			"EmailAddres": {"YourEmailAddress"}
		}
		Response
		{
			"ApiKey": "guid-here"
		}
		Content-Type: application/json
		Response: 201, 400 bad email, 409 duplicate user
		Notes: Will create a user with this email address and return their api key. use email address to recover api key

		6. Increase Blaance
		Put "http://localhost:56882/api/users/balances{amount}"
		Headers:
			Content-Type: application/json
			Authorization: {yourapikey} 
		Body:
		{
		}
		Content-Type: application/json
		Response: 202, 403 if bad apikey
		Notes: Increases a user's balance by the amount specified here. For instance if amount if 5 and balance is 5 balance will be upgraded to ten. Doesn't take a credit card number 
		which might be bad for business
