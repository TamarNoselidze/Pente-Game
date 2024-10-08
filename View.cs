using System;                      
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gdk;
using Gtk;
using Cairo;
using Color = Cairo.Color;
using static Gdk.EventMask;
using static Gtk.Orientation;



namespace PENTE{
    public class Area : DrawingArea {

        static Color brown = new Color(0.9, 0.8, 0.7),
                black = new Color(0, 0, 0),
                blue = new Color(0, 0.5, 0.9),
                red = new Color(1, 0, 0.2),
                green = new Color(0, 0.9, 0.2),
                yellow = new Color (1, 1, 0.3),
                purple = new Color (0.4, 0.1, 0.2);


        ImageSurface canvas;
        Game game { get; set; }
        public int numOfplayers = 1;
        bool comp_turn = false;
        public Area(Game game) {
            this.game = new Game (numOfplayers);
            SetSizeRequest(1300, 950);
            canvas = new ImageSurface(Format.Rgb24, 1300, 950);
            using (Context c = new Context(canvas)) {
                c.SetSourceColor(brown);
                c.Paint();
            }

            AddEvents((int) EventMask.ButtonPressMask);

        }
        
        protected override bool OnButtonPressEvent (EventButton click) {
            int x=-1; int y=-1;
            if ((75<=click.X) && click.X<=925 && 55<=click.Y && click.Y<=905 && game.game_over==false){
                x = (int) (click.X-75)/50;
                y = (int) (click.Y-55)/50;
                game.play(x,y);
                if (numOfplayers==1){
                    comp_turn=true;    
                }
            }

            QueueDraw();
            // comp_play(x,y);
            
            if (comp_turn){
                comp_play(x,y);
            }

            return true;
        }

        
        public void comp_play(int playerX, int playerY){
            game.comp_play(playerX, playerY);
                            Int64 i=0;

                while (i<999999){
                i++;
            }
                QueueDraw();
        }
        
        public void redraw (Context c){                       // if anything changed in board[][], redraw the area
                draw_board(c);
                for (int i=0; i<17; i++){
                    for (int j=0; j<17; j++){
                        if (game.board[j,i] > 0){    //there is a stone placed at this sqr
                            //draw a stone
                            int centre_x=50*j+100;
                            int centre_y=50*i+80;
                            switch (game.board[j,i]){
                                case 1:
                                    c.SetSourceColor(blue);
                                    break;
                                case 2:
                                    c.SetSourceColor(red);
                                    break;
                                case 3:
                                    c.SetSourceColor(green);
                                    break;
                                case 4: 
                                    c.SetSourceColor(yellow);
                                    break;
                            }
                            
                            c.Arc(xc: centre_x, yc: centre_y, radius:20, angle1: 0.0, angle2:2*Math.PI);
                            c.Fill();
                            
                        } 
                    } 
                }

                if (game.winner>0){
                    game_won(c);
                    game.game_over=true;
                }   

        }

        public void game_won(Context c){                      // draw a winning line; a window with a text indicating who won etc.

            //draw a line accross the winning row 
            int x1=game.win_x1*50+100;
            int y1=game.win_y1*50+80;
            int x2=game.win_x2*50+100;
            int y2=game.win_y2*50+80;
            c.SetSourceColor(purple);
            c.LineWidth = 5;
            c.MoveTo(x1, y1);
            c.LineTo(x2, y2);
            c.ClosePath();
            c.StrokePreserve();  


            //game over text 
            c.SetSourceColor(purple);
            c.Rectangle(x: 970, y: 550, width: 320, height: 220);
            c.Stroke();

            (int x, int y) = (1130, 600); 
            c.SetSourceColor(black);
            c.SetFontSize(30);
            string t1;
            string t2;
            if (game.winner!=10){
                t1 = "Game over.";
                if (numOfplayers == 2){
                    t2 = $"Player {game.winner} won.";
                }
                else if(numOfplayers == 1){
                    if (game.winner == 2){
                        t2 = $"Computer won.";
                    }
                    else{
                        t2 = $"Player won.";
                    }
                }
                else{
                    if (game.winner == 1 || game.winner ==3){
                        t2 = "Team 1 won.";
                    }
                    else{
                        t2 = "Team 2 won.";
                    }
                }
            }
            else{
                t1 = "The board is full and there";
                t2 = "are no more possible moves";
            }
            string t3 = "Click the Restart button";
            string t4 = "to start a new game.";

            string [] game_over = {t1, t2, t3, t4};      
            for (int i=0; i<4; i++){
                TextExtents t = c.TextExtents(game_over[i]);
                c.MoveTo(x+i - (t.Width / 2 + t.XBearing), y+i*40 - (t.Height / 2 + t.YBearing));
                c.ShowText(game_over[i]);
            }   
        }   

        public void reset_game(){                             // create a new Game class and reset the game
            game = new Game(numOfplayers);
            QueueDraw();
        }
        public void draw_board(Context c) {                   // default drawings for the board 
            c.SetSourceColor(black);
            c.LineWidth = 1;
            int start_x = 50;
            int start_y=30;
            int end_x=50;
            int end_y=930;
            int width = 50;
            for (int i=0; i<19; i++){
                //vertical lines
                c.MoveTo(start_x+i*width, start_y);
                c.LineTo(end_x+i*width, end_y);     

                //horizontal lines
                c.MoveTo(start_x, start_y+i*width);
                c.LineTo(950, 30+i*width);
                c.Stroke();
            }

            // player x-s turn TEXTBOX:
            
            (int cx, int cy) = (1100, 100);                     // center of rectangle
                string st;
                if (numOfplayers==1){
                    if (game.turn == 2){
                        st = "Computer's turn";
                    }
                    else{
                        st = "Player's turn";
                    }
                }
                else{
                    st= "Player " + game.turn +"'s turn: ";
                }
                c.SetFontSize(30);
                TextExtents te = c.TextExtents(st);
                c.MoveTo(cx - (te.Width / 2 + te.XBearing), cy - (te.Height / 2 + te.YBearing));
                c.ShowText(st);
                switch (game.turn){
                    case 1:
                        c.SetSourceColor(blue);
                        break;
                    case 2:
                        c.SetSourceColor(red);
                        break;
                    case 3: 
                        c.SetSourceColor(green);
                        break;
                    default:
                        c.SetSourceColor(yellow);
                        break;
                }
                c.Arc(xc: 1230, yc: 100, radius:18, angle1: 0.0, angle2:2*Math.PI);
                c.Fill();
            

            // Captures Text:
            (int dx, int dy) = (1100, 300);
            string capt = "Captures:";
            c.SetFontSize(40);
            c.SetSourceColor(black);
            TextExtents t = c.TextExtents(capt);
            c.MoveTo(dx - (t.Width / 2 + t.XBearing), dy - (t.Height / 2 + t.YBearing));
            c.ShowText(capt);

            // *stone* X {num_of_captures}
            if (numOfplayers==2 || numOfplayers==1){
                captures_tracker(c, 1, 1070, 350);
                captures_tracker(c, 2, 1130, 350);
            }
            else{
                (int x, int y) = (1050, 370);
                for (int i=0; i<2; i++){
                    string s = $"Team {1+i}:";
                    c.SetFontSize(25);
                    TextExtents tt = c.TextExtents(s);
                    c.MoveTo(x - (tt.Width / 2 + tt.XBearing), y+i*80 - (tt.Height / 2 + tt.YBearing));
                    c.ShowText(s);
                }
                captures_tracker(c, 1, 1120, 350);
                captures_tracker(c, 3, 1180, 350);
                captures_tracker(c, 2, 1120, 430);
                captures_tracker(c, 4, 1180, 430);
            }
            
        }
        void captures_tracker(Context c, int player, int x, int y){
            Color color;
            string stone_x;
            switch (player){
                case 1:
                    color = blue;
                    stone_x = $"x {game.player1_captures}";
                    break;
                case 2:
                    color = red;
                    stone_x = $"x {game.player2_captures}";
                    break;
                case 3:
                    color = green;
                    stone_x = $"x {game.player3_captures}";
                    break;
                default: 
                    color = yellow;
                    stone_x = $"x {game.player4_captures}";
                    break;
            }

            c.SetSourceColor(color);               //draw the stone
            c.Arc(xc: x, yc: y+33, radius:18, angle1: 0.0, angle2:2*Math.PI);
            c.Fill();
                                                    //number of captures
            c.SetFontSize(30);
            TextExtents t = c.TextExtents(stone_x);
            c.MoveTo(x - (t.Width / 2 + t.XBearing), y - (t.Height / 2 + t.YBearing));
            c.ShowText(stone_x);
        }

       
        protected override bool OnDrawn (Context c) {
            c.SetSourceSurface(canvas, 0, 0);
            c.Paint();
            redraw(c);
            return true;
        }
    }

    public class MyWindow : Gtk.Window {
        static int number_of_players;
        Area area;
        Button rules = new Button ("Rules");
        Button restart = new Button ("Restart");
        Button about = new Button ("About");

        public MyWindow() : base("Pente") {
            Box players = new Box(Horizontal, 5);
            RadioButton two = new RadioButton("2 players");
            RadioButton four = new RadioButton(two, "4 players");
            two.Clicked += two_players;
            four.Clicked += four_players;
            players.Add(two);
            players.Add(four);

            Box left_vbox = new Box(Vertical, 5);
            left_vbox.Add(players);
            left_vbox.Add(rules);
            left_vbox.Add(restart);
            left_vbox.Add(about);
            rules.Clicked += on_rules_clicked;
            restart.Clicked += on_restart_clicked;
            about.Clicked += on_about_clicked;
            Game game = new Game(number_of_players);
            area = new Area(game);

            Box main_box = new Box(Horizontal, 5);
            main_box.Add(left_vbox);
            main_box.Add(area);

            Add(main_box);

        }
        
        void on_rules_clicked (object? sender, EventArgs e){

            //dialog window with game rules
            string text = File.ReadAllText(
                System.IO.Path.Combine(Environment.CurrentDirectory, "rules.txt"));
            MessageDialog dw = new MessageDialog(this, 
                DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, text);
            dw.Run();
            dw.Destroy();
        }

        void on_about_clicked (object? sender, EventArgs e){
            string text = "Game created by Tamar Noselidze.\n"
                        + "Rules might differ from the original game.\n\n"
                        + "Main source for game implementation and rules: "
                        + "\nhttps://www.ultraboardgames.com/pente/game-rules.php";

            MessageDialog dw = new MessageDialog(this, 
                DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, text);
            dw.Run();
            dw.Destroy();
        }
       
        void on_restart_clicked(object? sender, EventArgs e){
            area.reset_game();
        }

        // void dummy_fun(){
        //    string text = "CLICKED";
        //     MessageDialog dw = new MessageDialog(this, 
        //         DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, text);
        //     dw.Run();
        //     dw.Destroy(); 
        // }
        void two_players (object? sender, EventArgs e){
            number_of_players = 2;
            // dummy_fun();
            // game = new Game (2);
            // area = new Area (game);
        }
         
        void four_players(object? sender, EventArgs e){
            number_of_players = 4;
            // game = new Game (4);
            // area = new Area (game);
        }

        protected override bool OnDeleteEvent(Event e) {
            Application.Quit();
            return true;
        }
    }

}
