Kennards Gomoku
Author Lee Arnould 0419896933 lee.arnould@gmail.com

A headless API to play the game Gomoku. a 15x15 board where tiles are added and the goal is to get 5 tiles in a row (horizontally, vertically or diagonally)

*To Test*
Open Solution ~kennards\gomoku\api\api.sln
Open Test Explorer (Test>TestExplorer)
Run 

Results 16 test should be successful

*To Run*
Run the solution
Then use the url in the output window

        CreateGame: [GET] http://localhost:7071/api/gomoku/v1/create

        GameMove: [GET] http://localhost:7071/api/gomoku/v1/move

Create Game doesn't need any parameters but Move requires a JSON Gomoku Object you can copy the one produced in create and then alter the CurrentMove component. (Or just run the tests is easier)


*Classes*
Gomoku
The game it includes the Board, Players and next Move (Intersection).
Board
The current board contains a List of Intersection, The GameStyle (Gomoku, LeemoKu, Tick Tack Toe) and the board state
InterSection
A playable position on a board containing the current occupied Player and the Row and Column
Player
A player at this stage just their name is stored
Players
A list of players
GameStyle
Stores the number of squares, the success pattern and the name of the game.
GameStyles
A collection of Games.

