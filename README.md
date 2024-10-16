# Osu Lobby Finder
Osu Lobby Finder is a console application designed to help you find multiplayer osu! lobbies quickly and efficiently. By specifying a few parameters, you can locate lobbies that match your criteria.

## Features
- Search for multiplayer osu! lobbies by specifying a starting ID, a time-to-live (TTL) limit, and a lobby name or pattern.
- Utilizes the osu! Legacy API to retrieve lobby information.
  
## Requirements
- An osu! Legacy API key. You can obtain this from the osu! website here: https://osu.ppy.sh/home/account/edit#legacy-api.

## Installation
Clone the repository:
```bash
git clone https://github.com/Dri3xCz/OsuLobbyFinder.git
cd OsuLobbyFinder
```
Build the project using your preferred C# IDE, such as Visual Studio.

## Usage
To use OsuLobbyFinder, you'll need to provide three parameters when running the program:

- Starting ID: The entry point for the program to begin searching for your desired lobby. It will start from this ID and search by decreasing the lobby ID.
- Time To Live (TTL): The maximum number of searches before the program stops.
- Name: The name or pattern of the lobby you are searching for.

Example
Run the executable and enter the parameters when prompted:

```plaintext
ID of the first lobby: 114582849
Time To Live (TTL): 250
Lobby Name: Example Lobby Name
```
This will start searching for a lobby starting from ID 114582849, performing up to 250 searches, and looking for lobbies with names that include "Example lobby name".

## Parameters
- Starting ID: (Required) The starting lobby ID for the search.
- Time To Live (TTL): (Required) The maximum number of searches the program will perform.
- Name: (Required) The name or partial name of the lobby you are looking for.

## API Key
OsuLobbyFinder requires an osu! Legacy API key to function. Follow these steps to obtain your key:

- Visit the osu! API key page: https://osu.ppy.sh/home/account/edit#legacy-api or enter GETAPI in application when prompted.
- Create a new LEGACY API key.
- Copy the API key and paste it into the program when prompted.

## Contributing
Contributions are welcome! Please open an issue or submit a pull request.
