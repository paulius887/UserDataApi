# UserDataApi
Web API with RESTful commands for accessing simple user data and text entries of these users:
### User data:
- Username
- Email
- RegisterDate (date of when user was first added)
- DisplayName (optional)
### User entries:
- EntryText (content of the entry)
- LastEdited (date of when entry was last edited)
# How to run
### Step 1: Clone the git repository
git clone ht<span>tps://</span>github.com/paulius887/UserDataApi
### Step 2: Build the project with Docker Compose
docker-compose build<br />
### Step 3: Run the project with Docker Compose
docker-compose up<br />
# Example JSONs
### JSON containing current user information, returned by GET command
```json
{
    "id": 1,
    "registerDate": "0001-01-01T00:00:00",
    "username": "string",
    "email": "string@gmail.com",
    "displayName": "string"
}
```
### JSON containing new/updated user information, used in POST/PUT commands
```json
{
    "username": "string",
    "email": "string@gmail.com",
    "displayName": "string"
}
```
### JSON containing current user entry information, returned by GET command
```json
{
    "userId": 1,
    "id": 1,
    "lastEdited": "0001-01-01T00:00:00",
    "entryText": "string"
}
```
### JSON containing new/updated user entry information, used in POST/PUT commands
```json
{
    "entryText": "string"
}
```
# Available commands
### GET
ht<span>tp://localhost:5000/api/Users/ - Get information of all users <br />
ht<span>tp://localhost:5000/api/Users/{id} - Get information of an user with specified id <br />
ht<span>tp://localhost:5000/api/Entries - Get information of all entries of all users <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries - Get information of all entries of an user with specified id <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries/{entryid} - Get information of an entry with specified entry id of an user with specified id
### POST
ht<span>tp://localhost:5000/api/Users/ - Create a new user <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries - Create a new entry of an user with specified id
### PUT
ht<span>tp://localhost:5000/api/Users/{id} - Update information of an user with specified id <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries/{entryid} - Update information of an entry entry with specified entry id of an user with specified id
### DELETE
ht<span>tp://localhost:5000/api/Users/{id} - Remove an user with specified id (will also delete all related entries) <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries/{entryid} - Remove an entry entry with specified entry id of an user with specified id
