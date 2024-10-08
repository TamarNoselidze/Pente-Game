using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PENTE{
  
    public class Game {            
        // each square: 0 = empty, 1 = player 1 stone, 2 = player 2 stone  ....

        int number_of_players;
        public int numOfstones = 0;        //number of stones of player 1, in case playing with the computer
        bool othermove = true;
        public bool board_updated = false;
        public int [,] board = new int [17 , 17];
        public int turn { get; set; }        // the player whose turn it is to move (1 or 2)
        public int player1_captures;  public int player2_captures;        // number of captures for each player
        public int player3_captures;  public int player4_captures;
        public int winner { get; set; }     // the player who has won, if any
        public  int win_x1{ get; set; } public int  win_y1 { get; set; }
        public int   win_x2 { get; set; } public int win_y2 { get; set; }     // (x,y) positions of the end and start of the winning line
        public bool game_over=false;
        (int,int) [] dirs = {(1, -1), (1, 0), (1, 1), (0, 1)}; 
        public Game(int n){
            number_of_players = n;
            if (number_of_players==4){
                this.player3_captures = this.player4_captures = 0;
            }

            this.turn = 1;                                     
            this.winner = 0;
            this.win_x1 = this.win_y1 = -1;
            this.win_x2 = this.win_y2 = -1;
            this.player1_captures = this.player2_captures = 0;
        }

        public bool board_full(){                              // check if there is no more empty intersection on the board
            for (int i=0; i<17; i++){
                for (int j=0; j<17; j++){
                    if(at(i,j)==0){
                        return false;
                    }
                }
            }
            return true;
        }

        int at(int x, int y){                                  // stone that is placed AT these coordinates 
            if(x>=0 && x<17 && y>=0 && y <17) {        // if returned 0 => no stone placed
                return board[x, y];
            }
            return -1;
        }

        bool five_in_a_row(int x, int y, int dx,  int dy){        // checks if there are 5 stones in a row, in ANY DIRECTION around the recently played stone
            int player_temp = at(x,y);
            
            if (player_temp>0){                                    //if there is a stone placed at this square
                if (at(x-dx, y-dy) != player_temp){                //the stone in the opposite dir. is different
                    if (check_five(x, y, dx, dy, player_temp)){
                        return true;
                    }
                }
            }
            return false;
        }
        
        bool check_five(int x, int y, int dx,  int dy, int player){     // checks if there are five stones in a row, in a GIVEN DIRECTION
            for (int i=1; i<5; i++){
                if (at(x+i*dx, y+i*dy)!=player){
                    return false;
                }
            }

            if (at(x+5*dx, y+5*dy)==player){     // OPTIONAL: win is exatcly five in a row (not a win for 6>)
                return false; 
            }

            return true;        
        }
        (int, int, int, int) two_in_a_row(int player){
            (int, int, int, int) result = (-1,-1, -1,-1);
            for(int i=0; i<17; i++){                      
                for (int j=0; j<17; j++){
                    if (at(i,j)==player){

                        foreach ((int,int) pair in dirs) {
                            if (check_two(i, j, pair.Item1, pair.Item2, player)){
                                result = (i, j, pair.Item1, pair.Item2);
                                return result;
                            }

                        }
                    }
                }
            }
            return result;
        }

        bool check_two (int x, int y, int dx, int dy, int player){
            for (int i=1; i<2; i++){
                if (at(x+i*dx, y+i*dy)!=player){
                    return false;
                }

                if (at(x+2*dx, y+2*dy)==player){
                    return false;
                }
            }
            return true;
        }
        bool check_three(int x, int y, int dx, int dy, int player){
            for (int i=1; i<3; i++){
                if (at(x+i*dx, y+i*dy)!=player){
                    return false;
                }

                if (at(x+3*dx, y+3*dy)==player || at(x-dx, y-dy)==player){
                    return false;
                }

                if (at(x+3*dx, y+3*dy)==next(player) && at(x-dx, y-dy)==next(player)){
                    return false;
                }
                
            }
            return true;
        }

        bool check_four(int x, int y, int dx, int dy, int player){
            for (int i=1; i<4; i++){
                if (at(x+i*dx, y+i*dy)!=player){
                    return false;
                }

                if (at(x+4*dx, y+4*dy)==player || at(x-dx, y-dy)==player){
                    return false;
                }

                if (at(x+4*dx, y+4*dy)==next(player) && at(x-dx, y-dy)==next(player)){
                    return false;
                }
            }
            return true;
        }


        (int, int, int, int) three_in_a_row(int player){
            (int, int, int, int) result = (-1,-1, -1,-1);
            for(int i=0; i<17; i++){                      
                for (int j=0; j<17; j++){
                    if (at(i,j)==player){
                        foreach ((int,int) pair in dirs){
                            if (check_three(i,j, pair.Item1, pair.Item2, player)){
                                result = (i,j, pair.Item1, pair.Item2);
                                return result;
                            }
                        }
                        
                    }
                }
            }
            return result;
        }

        (int, int, int, int) four_in_a_row(int player){
            (int, int, int, int) result = (-1,-1, -1,-1);
            for(int i=0; i<17; i++){                      
                for (int j=0; j<17; j++){
                    if (at(i,j)==player){
                        foreach ((int,int) pair in dirs){
                            if (check_four(i,j, pair.Item1, pair.Item2, player)){
                                result = (i,j, pair.Item1, pair.Item2);
                                return result;
                            }
                        }
                        
                    }
                }
            }
            return result;
        }


        int captures_winner (){                       // return the number of player (if any) that has >= 5 captures 
            if (player1_captures >= 5){    //OPTIONAL: not necessarily EXACTLY 5 captures
                return 1;
            }
            if (player2_captures >= 5){
                return 2;
            }
            if (number_of_players == 4){
                if (player3_captures + player1_captures >= 10){
                    return 3;
                }
                if (player4_captures + player2_captures >= 10){
                    return 4;
                }

            }

            return -1;
        }

        int check_win(){                              // check if anyone won

            if (captures_winner()>0){                     //check if any player has 5> captures
                winner = captures_winner();         
                return winner;
            }

            for(int x=0; x<17; x++){                      //check five_in_a_row for EACH square
                for (int y=0; y<17; y++){
                    foreach ((int,int) pair in dirs) {
                        if (five_in_a_row(x, y, pair.Item1, pair.Item2)){
                            winner = next(turn);                    
                            win_x1=x;           
                            win_y1=y;
                            win_x2=x+4*pair.Item1;
                            win_y2=y+4*pair.Item2;
                            return winner;
                        }
                    }
                }
            }

            //check if the board is full 
            if (board_full()){
                winner = 10;                     // პირობითად 10, მაგ. 0ზე არ მოქმედებდა
                return winner;
            }
            return -1;
        }

     
        int next(int player){                         // alternate the turn
            int result = player % number_of_players + 1;
            if (number_of_players==1){
                result = player % 2 +1;
            }
            return result;
        }

        bool valid_move(int x, int y){                // check if the move made is valid
            if (at(x,y) == 0){
                return true;                
            }
            else{
                return false;
            }
        }

        void check_captures(int x, int y){            // coordinates of the last placed stone
            //find the starting points of the 4 rows intersecting at this square
            (int, int) [] start_points = new (int,int) [4]; 

            for (int i=0; i<4; i++){
                int tmp_x=x;
                int tmp_y=y;
                (int,int) pair = dirs[i];
                for (int j=0; j<3; j++){    
                    if (tmp_x-pair.Item1>=0 && tmp_y-pair.Item2>=0){
                        tmp_x-=pair.Item1;
                        tmp_y-=pair.Item2;
                    }
                } 
                start_points[i]=(tmp_x,tmp_y);
            }


            //check the rows for captures
            for (int k=0; k<4; k++){
                (int, int) dir = dirs[k];
                (int, int) pt = start_points[k];

                int prev_stone = at(pt.Item1, pt.Item2);
                (int, int) prev_sqr = (pt.Item1, pt.Item2);

                for (int l=1; l<5; l++){
                    (int, int) curr_sqr = (prev_sqr.Item1+dir.Item1, prev_sqr.Item2+dir.Item2);
                    int curr_stone = at(curr_sqr.Item1, curr_sqr.Item2);

                    if (prev_stone >0 && curr_stone >0 && curr_stone != prev_stone){

                        if (at(curr_sqr.Item1+dir.Item1, curr_sqr.Item2+dir.Item2) == curr_stone &&
                            at(curr_sqr.Item1+2*dir.Item1, curr_sqr.Item2+2*dir.Item2) == prev_stone){
                            //found a capture
                                switch (prev_stone){
                                    case 1:
                                        player1_captures+=1;
                                        break;
                                    case 2:
                                        // Console.WriteLine("found a capture!");
                                        player2_captures+=1;
                                        if (number_of_players==1){
                                            numOfstones-=2;
                                        }
                                        break;
                                    case 3:
                                        player3_captures+=1;
                                        break;
                                    case 4:
                                        player4_captures+=1;
                                        break;
                                }
                                
                                //update board
                                board[curr_sqr.Item1, curr_sqr.Item2] = 0;
                                board[curr_sqr.Item1+dir.Item1, curr_sqr.Item2+dir.Item2] = 0;

                        }   
                    }
                    prev_sqr = curr_sqr;
                    prev_stone = curr_stone;
                }
            }
        }

        void reset_board(){                           // empty the board
            for (int i=0; i<17; i++){
                for (int j=0; j<17; j++){
                    board[i,j] = 0;
                }
            }

            turn = 1;
        }
        
        public bool play(int x, int y){
            if (valid_move(x,y)){
                board[x, y]=turn;
                // Console.WriteLine($"Put stone {turn} at coordinates {(x,y)}");
                if (number_of_players ==1 && turn==1){

                    numOfstones+=1;
                }
                turn=next(turn);
                // Console.WriteLine($"Now it's {turn}'s turn");
                check_captures(x,y);
                if(check_win()>0){
                    winner=check_win();
                    game_over=true;
                }
                return true;
            }
            return false;
        }

        bool comp_for_two(int x, int y, int dx, int dy){
            //case 1: there is a computer's stone placed on one end of the pair, so now we place a stone on another and capture the pair
            if (at((x-dx), (y-dy)) == 2) {
                play((x+2*dx), (y+2*dy));
                return true;
            }
            if (at((x+2*dx), (y+2*dy)) == 2){
                play((x-dx), (y-dy));  
                return true;                  
            }
            //case 2: both ends are empty
            if (at((x-dx), (y-dy)) == 0 && at((x+2*dx), (y+2*dy)) == 0){
                bool temp = play((x+2*dx), (y+2*dy));
                return true;
            }
            return false;
        }

        bool comp_for_three(int x, int y, int dx, int dy){
            //case 1: 
            if (at((x-dx), (y-dy)) == 2) {
                play((x+3*dx), (y+3*dy));
                return true;
            }
            if (at((x+3*dx), (y+3*dy)) == 2){
                play((x-dx), (y-dy));      
                return true;              
            }
            //case 2: both ends are empty
            if (at((x-dx), (y-dy)) == 0 && at((x+3*dx), (y+3*dy)) == 0){
                bool temp = play((x+3*dx), (y+3*dy));
                return true;        
            }

            return false;
        }

        bool comp_for_four (int x, int y, int dx, int dy){

            if (at((x-dx), (y-dy)) == 2) {
                play((x+4*dx), (y+4*dy));
                return true;
            }
            if (at((x+4*dx), (y+4*dy)) == 2){
                play((x-dx), (y-dy));      
                return true;              
            }
            //case 2: both ends are empty
            if (at((x-dx), (y-dy)) == 0 && at((x+4*dx), (y+4*dy)) == 0){
                play((x+4*dx), (y+4*dy));
                return true;        
            }            
            return false;
        }

    
        bool helper1((int, int, int, int) sqr, (int, int, int, int) sqr1, (int, int, int, int) sqr2){
            bool x=false;
            if (sqr.Item1!=-1){
                x=comp_for_four (sqr.Item1, sqr.Item2, sqr.Item3, sqr.Item4);
            }
            if (sqr.Item1==-1 && sqr2.Item1!=-1){
                    x=comp_for_three(sqr2.Item1, sqr2.Item2, sqr2.Item3, sqr2.Item4);
            }
            if(sqr.Item1==-1 && sqr2.Item1==-1 && sqr1.Item1!=-1){
                x=comp_for_two(sqr1.Item1, sqr1.Item2, sqr1.Item3, sqr1.Item4);
            }
            
            return x;
        }
        bool helper2((int, int, int, int) sqr3, (int, int, int, int) sqr4, (int, int, int, int) sqr5){
            bool x=false;
            if(sqr3.Item1!=-1 || sqr4.Item1!=-1 || sqr5.Item1!=-1){
                if(sqr3.Item1!=-1){
                    x=play(sqr3.Item1+4*sqr3.Item3, sqr3.Item2 + 4*sqr3.Item4);
                }
                else if (sqr3.Item1==-1 && sqr4.Item1!=-1){
                    x=play(sqr4.Item1+3*sqr4.Item3, sqr4.Item2 + 3*sqr4.Item4);
                }
                else if (sqr3.Item1==1 && sqr4.Item1==1){
                    x=play(sqr5.Item1+2*sqr5.Item3, sqr5.Item2 + 2*sqr5.Item4);
                }
            }
            return x;
        }


        public bool comp_play(int playerX, int playerY){

            if (numOfstones==1){                      //if there is only 1 stone of the player on the board, put a stone on any of the surrounding intersections
                Random rnd = new Random();
                int tempx=-1; int tempy=-1;
                while (!valid_move(tempx, tempy)){
                    int temp = rnd.Next(0, 4);
                    (int, int) temp_dir = dirs[temp];
                    tempx=playerX+temp_dir.Item1;
                    tempy=playerY+temp_dir.Item2; 
                }
                 
                play(tempx, tempy);
                return true;
            }
            else{
                (int, int, int, int) sqr = four_in_a_row(1);
                (int, int, int, int) sqr1 = two_in_a_row(1);
                (int, int, int, int) sqr2 = three_in_a_row(1);
                (int, int, int, int) sqr3 = four_in_a_row(2);
                (int, int, int, int) sqr4 = three_in_a_row(2);
                (int, int, int, int) sqr5 = two_in_a_row(2);
               
                if (sqr3.Item1!=-1){
                    play(sqr3.Item1+4*sqr3.Item3, sqr3.Item2+4*sqr3.Item4);
                }
                bool x1=helper1(sqr, sqr1, sqr2);
                bool x2=helper2(sqr3, sqr4, sqr5);
                
                if (!x1 && !x2){
                    for (int m=0; m<17; m++){
                        for (int n=0; n<17; n++){
                            if (at(m,n) == 2){

                                Random rnd = new Random();
                                int tempx=-1; int tempy=-1;
                                while (!valid_move(tempx, tempy)){
                                    int temp = rnd.Next(0, 4);
                                    (int, int) temp_dir = dirs[temp];
                                    tempx=playerX+temp_dir.Item1;
                                    tempy=playerY+temp_dir.Item2; 
                                }
                        
                                play(tempx, tempy);
                                return true;
                            }
                        }
                    }
                }
            }
            return true;
        }
        
    }
}