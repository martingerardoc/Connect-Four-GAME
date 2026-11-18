namespace Connect_Four_GAME.Components.Models;

public class GameState
{
    public const int Rows = 6;
    public const int Columns = 7;

    // 0 = empty, 1 = Player 1, 2 = Player 2
    public int[] Pieces { get; private set; } = new int[Rows * Columns];

    public int CurrentPlayer { get; private set; } = 1;

    public bool GameOver { get; private set; }

    public int Winner { get; private set; }

    public void ResetGame()
    {
        Pieces = new int[Rows * Columns];
        CurrentPlayer = 1;
        GameOver = false;
        Winner = 0;
    }

    public bool PlayPiece(int column)
    {
        if (GameOver || column < 0 || column >= Columns)
        {
            return false;
        }

        // Find the lowest available position in the selected column.
        for (int row = Rows - 1; row >= 0; row--)
        {
            int position = row * Columns + column;

            if (Pieces[position] == 0)
            {
                Pieces[position] = CurrentPlayer;

                // Check whether this move wins the game.
                if (CheckWinner(row, column))
                {
                    Winner = CurrentPlayer;
                    GameOver = true;
                }
                else if (Pieces.All(piece => piece != 0))
                {
                    // The board is full: the game is a draw.
                    GameOver = true;
                }
                else
                {
                    // Switch players.
                    CurrentPlayer = CurrentPlayer == 1 ? 2 : 1;
                }

                return true;
            }
        }

        // The selected column is full.
        return false;
    }

    private bool CheckWinner(int row, int column)
    {
        int player = CurrentPlayer;

        // Horizontal, vertical, and both diagonal directions.
        int[][] directions =
        [
            [0, 1],
            [1, 0],
            [1, 1],
            [1, -1]
        ];

        foreach (int[] direction in directions)
        {
            int count = 1;

            count += CountPieces(row, column,
                direction[0], direction[1], player);

            count += CountPieces(row, column,
                -direction[0], -direction[1], player);

            if (count >= 4)
            {
                return true;
            }
        }

        return false;
    }

    private int CountPieces(
        int row, int column,
        int rowDirection, int columnDirection,
        int player)
    {
        int count = 0;

        int currentRow = row + rowDirection;
        int currentColumn = column + columnDirection;

        while (
            currentRow >= 0 &&
            currentRow < Rows &&
            currentColumn >= 0 &&
            currentColumn < Columns &&
            Pieces[currentRow * Columns + currentColumn] == player)
        {
            count++;
            currentRow += rowDirection;
            currentColumn += columnDirection;
        }

        return count;
    }
}

