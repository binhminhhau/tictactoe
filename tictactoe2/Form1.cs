using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tictactoe2
{
    public partial class Form1 : Form
    {
        private char[] board = new char[9]; // Bàn cờ 3x3
        private char currentPlayer = 'X'; // Người chơi bắt đầu
        public Form1()
        {
            InitializeComponent();
            InitializeBoard();         //gọi hàm khởi tạo bảng bên dưới 


            
        }
        

        private void InitializeBoard() //khởi tạo bảng
        {
            for (int i = 0; i < 9; i++)
            {
                board[i] = ' ';                         //với mỗi ô là 1 khoảng rỗng 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }




        private void button_Click(object sender, EventArgs e)   //tạo hàm phản ứng với mỗi click từ người dùng (giao diện)
        {
            
            Button btn = sender as Button;                      //xử lí event trong c#
            int index = int.Parse(btn.Name.Replace("button", "")); // Lấy chỉ số từ tên button (vị trí của ô đó trong bảng)
            if (index >= 0  && index < board.Length&& board[index] == ' ')  
            { //nếu vị trí lớn hơn 0 đồng thời vị trí nhỏ hơn độ dài của bảng(9) và vị trí đó trống
                
                //kiểm tra xem có phải thắng hay thua rồi không
                MakeMove(index, currentPlayer);            //gọi hàm mke move bên dưới và truyền vào vị trí, lượt của ai 
                    if (CheckWinner(currentPlayer))        //kiểm tra người có thắng hay không 
                    {
                        MessageBox.Show($"Player {currentPlayer} wins");    //nếu có người thắng thì thông báo message box

                    //  ResetGame();
                    DisableAllButtons();               //khi có người thắng thì sẽ khóa tất cả các ô lại không cho đánh tiếp
                    return;
                    }
                    if (IsDraw())        //nếu là hòa thì chỉ đưa ra thông báo, không cần khóa các ô lại vì hết ô rồi 
                    {
                        MessageBox.Show("It's a draw!");
                        //ResetGame();
                        return;
                    }
                


                    
                    currentPlayer = 'O'; //  chuyển qua lượt của  Máy chơi
                    int bestMove = FindBestMove();      //gọi hàm tìm nước đi tốt nhất để gán cho bestMove
                    if (bestMove != -1) // Nếu tìm được nước đi hợp lệ     
                    {

                        MakeMove(bestMove, currentPlayer);  //gọi hàm di chuyển và truyền vào nước đi tôt nhất và lượt chơi hiện tại(máy)

                        if (CheckWinner(currentPlayer))  //kiểm tra máy thắng hay chưa
                        {
                            MessageBox.Show("Computer wins!");
                        //isResetting = true;
                        // isResetting = false;
                        DisableAllButtons();  //khóa các nút

                        return;
                        }
                        if (IsDraw())     //nếu hòa thì ra thông báo bằng message box
                        {

                       DialogResult result= MessageBox.Show("It's a draw!","Thông báo",MessageBoxButtons.OK);
                        if (result == DialogResult.OK)
                        {
                            ResetGame();
                        }    
                       
                            return; // Ngừng xử lý nếu hòa
                        }

                    }


                    // Đổi lại lượt cho người chơi
                    currentPlayer = 'X';
                }
            
        } 
        

                private bool IsDraw()    //hàm kiểm tra hòa 
                {
                    foreach (char c in board)  //dùng vòng lặp qua tất cả các ô, xem còn ô nào trống ko
                    {
                        if (c == ' ') return false;    //không có thì trả về false 
                    }
                    return true;                      //có thì true
                }

                private int FindBestMove()      //hàm tìm nước đi tốt nhất 
                {
                    int bestScore = int.MinValue;    //khởi tạo điểm cao nhất là âm vô cùng
                    int move = -1;                  

                    for (int i = 0; i < 9; i++)
                    {
                        if (board[i] == ' ')    //duyệt qua các ô còn trống
                        {
                            board[i] = 'O'; // Máy đánh thử vào trường hợp ô trống
                            int score = Minimax(board, false);  //gọi hàm minimax để minimax đánh giá điểm của các nước đi trong tương lai người chơi
                            board[i] = ' '; // Hoàn lại ô trống sau khi minimax đánh giá xong

                            if (score > bestScore) //nếu điểm sau(score) lớn hơn điểm trước (bestscore) thì sẽ gán điểm sau là điểm tốt nhất
                            {
                                bestScore = score;
                                move = i; //đi nước đi i này 
                            }
                        }
                    }

                    return move;  //trả về nước đi vào ô i vừa tìm được
                } 

                private int Minimax(char[] boardState, bool isMaximizing)  // hàm minimax (thuật toán cốt lõi của game, trả về điểm sau khi đánh giá_)
                {
                    if (CheckWinner('O')) return 1;  //nếu máy thắng thì trả về 1
                    if (CheckWinner('X')) return -1; //người thắng thì điểm là -1
                    if (IsDraw()) return 0;         //hòa thì điểm là 0

                    if (isMaximizing)    //nếu là lượt của máy thì sẽ tối đa hóa lợi nhuận (tấn công)
                    {
                        int bestScore = int.MinValue; //khởi tạo điểm tốt nhất là âm vô cùng
                        for (int i = 0; i < 9; i++)    //duyệt qua tất cả các ô 
                        {
                            if (boardState[i] == ' ')    //tìm dc ô trống
                            {
                                boardState[i] = 'O';     //đánh vào ô đó
                                int score = Minimax(boardState, false);   //gọi hàm đệ quy vào các nút dưới sau đó gán vào điểm 
                                boardState[i] = ' ';        //hoàn lại nước đi 
                                bestScore = Math.Max(score, bestScore);   //so sánh điểm vào điểm tốt nhất để gán giá trị lớn hơn vào bestscore
                            }
                        }
                        return bestScore;   //trả về điểm tốt nhất 
                    }
                    else    //lượt của người chơi thì máy sẽ giảm tối đa lợi nhuận(phòng thủ)
                    {
                        int bestScore = int.MaxValue;    //khởi tạo giá trị lớn nhất là dương vô cùng để tất cả giá trị đầu tiên đều nhỏ hơn nó
                        for (int i = 0; i < 9; i++)
                        {
                            if (boardState[i] == ' ')
                            {
                                boardState[i] = 'X';
                                int score = Minimax(boardState, true);  //gọi đệ quy các nhánh dưới
                                boardState[i] = ' ';
                                bestScore = Math.Min(score, bestScore);   //lấy nước đi nhỏ nhất- bất lợi nhất cho người chơi 
                            }
                        }
                        return bestScore;
                    }
                }
            

        private void ResetGame()  //hàm reset
        {

            InitializeBoard();  //gọi hàm tạo bảng trống ở trên 
            foreach (Button btn in tableLayoutPanel1.Controls) //thêm table vào mới đúng, vì phải lấy từ bảng ra
            {
                if (btn is Button)    
                {
                    btn.Text = "";// Xóa nội dung của nút
                    btn.Enabled = true; // Kích hoạt lại nút
                }
            }
            currentPlayer = 'X'; // Người bắt đầu lại
        }
        
        private bool CheckWinner(char player)   //hàm kiểm tra người thắng 
        {
            int[,] winPatterns = new int[,] {
                { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 }, // Hàng ngang
                { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 }, // Hàng dọc
                { 0, 4, 8 }, { 2, 4, 6 }  // Đường chéo                 
            };
            Console.WriteLine(winPatterns[0,0]);      
            for (int i = 0; i < 8; i++)               //duyệt 8 TH ở trên 
            {      
                if (board[winPatterns[i, 0]] == player &&         
                    board[winPatterns[i, 1]] == player &&
                    board[winPatterns[i, 2]] == player)
                {
                    
                    return true;
                }
            }

            return false;
        }

        private void MakeMove(int index, char player) //hàm tạo nước đi 
        {
            board[index] = player;  //đổi ô index thành x hoặc o 
            Button btn =tableLayoutPanel1.Controls["button" + index] as Button; //đổi tên buttonx vừa chọn thành button để lấy nút đó
            btn.Text = player.ToString();    //chuyển đổi kí tự của người chơi thành chuỗi mới gán vào text được 
        }
        private void DisableAllButtons()
        {
            foreach (Button btn in tableLayoutPanel1.Controls)
            {
                if (btn is Button)
                {
                    btn.Enabled = false;
                    
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
          
           ResetGame();
        }
    }
}
