# Project Pente
===============


## Project description:

    The aim of the project is to recreate the well known abstract strategy board game - Pente.
    The project implements the game with slightly modified rules, namely: player wins if they have either exactly 5 stones in a row
    in horizontal, vertical or diagonal direction, OR if they have 5 or more captures. 
        - Captures consist of exactly two stones; flanking a single stone or three or more stones does not result in a capture. 
        
    
    The application offers the game for 4 players as well, in which case players pair up in two teams and one team wins when:
        - One player gets five (or more) of his or her own stones in a row, just as in basic-two player Pente, or
        - The team captures 10 (or more) stones from their opponents.


    The application is structured using a model-view architecture. Specifically, there are 3 C# files:

        - Program: This is where the function _main_ is located, and therefore where the main call of the application happens.
        - Model: A separate class _game_, which holds the state of the game. The rules of the game (e.g. checking whether a move is valid, 
                 and checking for a win) are all in this class. 
                 This class knows nothing about the user interface. Which means, it does not contain any GTK / GDK code.
        - View: Consists of two classes: 1) MyWindow, that is the base window for the application, and
                                         2) Area, which serves as the canvas that the board is drawn on.
                
                View does not contain any code that sets values directly in class Game's attributes. After the user makes a move, that is,
                clicks on the board, it is "communicated" to the Model structure through a function *game.play(x,y)* .


## Challenges and Modification Opportunities:

    The challenge I faced:
        The project overall was not difficult to implement, but the part I found a bit challenging was for detecting captures.
        - In this version of the application, moving into a "Captured Position", that is, putting a stone on the board so that it forms a  pair    between two enemy stones, is also considered as a capture. Usually this is a safe move, so an additional feature to implement
        in the future would be to modify the algorithm for detecting the captures and creating a special case for this move.

        - Again, checking for captures, namely, the method *check_captures* can be optimized.
        This version uses a detailed, very rigorous search for the captures in each direction of the played stone.




## How to Install and Run the Project:

    The project is written in C# language, using GTK interface. Therefore, there are severeal ways to compile and execute it, but the simplest one
    is by using cmd (Command Prompt). Below steps demonstrate how to run a C# program on Command line in Windows Operating System:

        - Open the cmd and run the command *csc* to check for the compiler version. It specifies whether you have installed a valid compiler or not. You can avoid this step if you confirmed that compiler is installed.
        - Now, simply type the filename i.e PROJECT on the cmd and it will give the output. OR, user can go to the directory where the program is saved and simply double-click the file and it will give the output.
            



## How to Use the Project:
    
    - The application can be used by either 2 players or 4 players.
        In each case, players take turns (with different stones) and place stones one at a time. The first player/team
        to get five stones in a row (in horizontal, vertical or diagonal direction) wins the game.

        For more detailed information about the rules, the user can click the "Rules" button in the application. 





## Credits:

    The application is created by Tamar Noselidze.
    Main source for game implementation and rules: https://www.ultraboardgames.com/pente/game-rules.php .

## License:

    GPL License.
    Readers / users are allowed to make modification to the code and use it for commercial purposes.


