# UserDataApi
Web API with RESTful commands for accessing simple user data and text entries of these users:
### User data:
- Username
- Email
- RegisterDate (date of when user was first added)
- DisplayName (optional)
### User entries:
- EntryText (content of the entry)
- Book (the book which the entry is talking about)
- LastEdited (date of when the entry was last edited)
# How to run
### Step 1: Clone the git repository
git clone --recursive ht<span>tps://</span>github.com/paulius887/UserDataApi
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
    "entry": {
      "entryText": "string",
      "userId": 0,
      "id": 0,
      "bookId": 0,
      "lastEdited": "0001-01-01T00:00:00"
    },
    "book": {
      "id": 0,
      "title": "string",
      "isbn": "string",
      "createdDate": "0001-01-01T00:00:00",
      "author": {
        "id": 0,
        "name": "string",
        "surname": "string"
      },
      "description": "string",
      "isAvailable": true,
      "unavailableUntil": "0001-01-01T00:00:00"
    }
  }
```
### JSON containing new user entry information, used in POST command
```json
{
  "entryDto": {
    "entryText": "string"
  },
  "bookDto": {
    "title": "string",
    "isbn": "string",
    "createdDate": "2023-05-09T14:24:44.257Z",
    "authorId": 0,
    "description": "string",
    "isAvailable": true,
    "unavailableUntil": "2023-05-09T14:24:44.257Z"
  }
}
```
### JSON containing updated user entry information, used in PUT command
```json
{
    "entryText": "string"
}
```
# Available commands
### GET
ht<span>tp://localhost:5000/api/Users/ - Get information of all users <br />
ht<span>tp://localhost:5000/api/Users/{id} - Get information of an user with specified id <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries - Get information of all entries of an user with specified id <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries/{entryid} - Get information of an entry with specified entry id of an user with specified id <br />
ht<span>tp://localhost:5000/api/Entries - Get information of all entries of all users <br />
### POST
ht<span>tp://localhost:5000/api/Users/ - Create a new user <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries - Create a new entry of an user with specified id <br />
### PUT
ht<span>tp://localhost:5000/api/Users/{id} - Update information of an user with specified id <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries/{entryid} - Update information of an entry entry with specified entry id of an user with specified id <br />
### DELETE
ht<span>tp://localhost:5000/api/Users/{id} - Remove an user with specified id (will also delete all related entries) <br />
ht<span>tp://localhost:5000/api/Users/{id}/Entries/{entryid} - Remove an entry entry with specified entry id of an user with specified id
