# UserDataApi
Web API with RESTful commands for accessing simple user data:
- Username
- Email
- RegisterDate (date of when user was first added)
- DisplayName (optional)
## How to run
### Step 1: Clone the git repository
git clone --recurse ht<span>tps://</span>github.com/paulius887/UserDataApi
### Step 2: Build the project with Docker Compose
docker-compose build<br />
### Step 3: Run the project with Docker Compose
docker-compose up<br />
## Example JSONs
### JSON containing current user information, returned by GET command
```json
{
    "id": 1,
    "registerDate": "0001-01-01T00:00:00",
    "username": "string",
    "email": "string@gmail.com"
    "displayName": "string"
}
```
### JSON containing new/updated user information, used in POST/PUT commands
```json
{
    "username": "string",
    "email": "string@gmail.com"
    "displayName": "string"
}
```
## Available commands
### GET
ht<span>tp://localhost:5000/api/Users/ - Get information of all users <br />
ht<span>tp://localhost:5000/api/Users/{id} - Get information of an user with specified id
### POST
ht<span>tp://localhost:5000/api/Users/ - Create a new user <br />
### PUT
ht<span>tp://localhost:5000/api/Users/{id} - Update information of an user with specified id <br />
### DELETE
ht<span>tp://localhost:5000/api/Users/{id} - Remove an user with specified id
