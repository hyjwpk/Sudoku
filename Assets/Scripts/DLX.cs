using System.Collections.Generic;

public class Node
{
    public Node up; public Node down; public Node left; public Node right; public Node colRoot; public Node rowRoot;
    public int Num; public int Size;//行数 列元素数
    public Node(int i = -1) //构造函数
    {
        Num = i;
        Size = 0;
    }
};

public class DLX
{
    private Node Head;
    private List<int> result = new List<int>();
    private int _row, _col;
    public List<List<int>> solve = new List<List<int>>();
    public DLX(ref List<List<int>> matrix, int m, int n)//构造函数
    {
        _row = m;
        _col = n;
        Head = new Node();
        Head.up = Head;
        Head.down = Head;
        Head.right = Head;
        Head.left = Head;
        init();
        link(ref matrix);
    }

    private void init()//初始化行列
    {
        Node newNode;
        for (int ix = 0; ix < _col; ++ix) //表头向后构造列对象
        {
            newNode = new Node();
            newNode.up = newNode;
            newNode.down = newNode;
            newNode.right = Head.right;
            newNode.left = Head;
            newNode.right.left = newNode;
            Head.right = newNode;
        }
        for (int ix = 0; ix < _row; ++ix) //表头向下构造行对象
        {
            newNode = new Node(_row - ix);
            newNode.down = Head.down;
            newNode.up = Head;
            newNode.down.up = newNode;
            Head.down = newNode;
        }
    }

    private void link(ref List<List<int>> matrix)//连接各个节点
    {
        Node current_row = Head, current_col, newNode, current; //当前行对象,当前列对象,新节点,当前节点
        for (int row = 0; row < _row; ++row)
        {
            current_row = current_row.down; current_col = Head;
            for (int col = 0; col < _col; ++col)
            {
                current_col = current_col.right;

                if (matrix[row][col] == 0)
                    continue;

                newNode = new Node();

                newNode.colRoot = current_col;
                newNode.rowRoot = current_row;

                newNode.down = current_col;
                newNode.up = current_col.up;
                newNode.up.down = newNode;
                current_col.up = newNode;

                if (current_row.Size == 0)
                {
                    current_row.right = newNode;
                    newNode.left = newNode;
                    newNode.right = newNode;
                    current_row.Size++;
                }
                current = current_row.right;
                newNode.left = current.left;
                newNode.right = current;
                newNode.left.right = newNode;
                current.left = newNode;

                current_col.Size++;
            }
        }
    }

    private void cover(ref Node cRoot) //覆盖列
    {
        cRoot.left.right = cRoot.right;
        cRoot.right.left = cRoot.left; //删除该列对象
        Node i = cRoot.down, j;
        while (i != cRoot)
        {
            j = i.right;
            while (j != i)
            {
                j.down.up = j.up;
                j.up.down = j.down;
                j.colRoot.Size--;
                j = j.right;
            }
            i = i.down;
        }
    }

    private void recover(ref Node cRoot) //恢复列
    {
        Node i = cRoot.up, j;
        while (i != cRoot)
        {
            j = i.left;
            while (j != i)
            {
                j.colRoot.Size++;
                j.down.up = j;
                j.up.down = j;
                j = j.left;
            }
            i = i.up;
        }
        cRoot.right.left = cRoot;
        cRoot.left.right = cRoot;
    }

    public bool Search(int k = 0)//搜索求解
    {
        if (Head.right == Head)
            return true;

        Node cRoot = new Node(), c;
        int minSize = int.MaxValue;
        for (c = Head.right; c != Head; c = c.right) //启发式搜索
        {
            if (c.Size < minSize)
            {
                minSize = c.Size;
                cRoot = c;
                if (minSize == 1)
                    break;
                if (minSize == 0)
                    return false;
            }
        }
        cover(ref cRoot);

        Node current_row, current;
        for (current_row = cRoot.down; current_row != cRoot; current_row = current_row.down)
        {
            result.Add(current_row.rowRoot.Num);
            for (current = current_row.right; current != current_row; current = current.right)
            {
                cover(ref current.colRoot);
            }
            if (Search(k + 1))
                return true;
            for (current = current_row.left; current != current_row; current = current.left)
                recover(ref current.colRoot);
            result.RemoveAt(result.Count - 1);
        }
        recover(ref cRoot);
        return false;
    }

    public void Searchall(int k = 0)//搜索求全部解，通过返回的解的数量判断是否有解，无bool返回值
    {
        if (solve.Count >= 100) return;
        if (Head.right == Head)
        {
            solve.Add(new List<int>(result));
            return;
        }

        Node cRoot = new Node(), c;
        int minSize = int.MaxValue;
        for (c = Head.right; c != Head; c = c.right) //启发式搜索
        {
            if (c.Size < minSize)
            {
                minSize = c.Size;
                cRoot = c;
                if (minSize == 1)
                    break;
                if (minSize == 0)
                    return;
            }
        }
        cover(ref cRoot);

        Node current_row, current;
        for (current_row = cRoot.down; current_row != cRoot; current_row = current_row.down)
        {
            result.Add(current_row.rowRoot.Num);
            for (current = current_row.right; current != current_row; current = current.right)
            {
                cover(ref current.colRoot);
            }
            Searchall(k + 1);
            for (current = current_row.left; current != current_row; current = current.left)
                recover(ref current.colRoot);
            result.RemoveAt(result.Count - 1);
        }
        recover(ref cRoot);
    }

    static public List<List<int>> sudoku2matrix(string problem) //将数独转换为01矩阵
    {
        List<List<int>> matrix = new List<List<int>>();
        for (int ix = 0; ix < 81; ++ix)
        {
            int val = problem[ix];
            List<int> current_row = new List<int>();
            //for (int i = 1; i <= 324; i++) current_row.Add(0);
            for (int i = 1; i <= 342; i++) current_row.Add(0); //x数独
            if (val != 0)
            {
                current_row[ix] = 1;
                current_row[81 + ix / 9 * 9 + val - 1] = 1;
                current_row[162 + ix % 9 * 9 + val - 1] = 1;
                current_row[243 + (ix / 9 / 3 * 3 + ix % 9 / 3) * 9 + val - 1] = 1;
                if (ix / 9 == ix % 9)//x数独
                    current_row[324 + val - 1] = 1; //x数独
                if (ix / 9 + ix % 9 == 8)//x数独
                    current_row[333 + val - 1] = 1; //x数独
                matrix.Add(current_row);
                continue;
            }
            for (int jx = 0; jx < 9; ++jx)
            {
                List<int> current_row2 = new List<int>();
                //for (int i = 1; i <= 324; i++) current_row2.Add(0); 
                for (int i = 1; i <= 342; i++) current_row2.Add(0); //x数独
                current_row2[ix] = 1;
                current_row2[81 + ix / 9 * 9 + jx] = 1;
                current_row2[162 + ix % 9 * 9 + jx] = 1;
                current_row2[243 + (ix / 9 / 3 * 3 + ix % 9 / 3) * 9 + jx] = 1;
                if (ix / 9 == ix % 9)//x数独
                    current_row2[324 + jx] = 1; //x数独
                if (ix / 9 + ix % 9 == 8)//x数独
                    current_row2[333 + jx] = 1; //x数独
                matrix.Add(current_row2);
            }
        }
        return matrix;
    }

    public List<int> matrix2sudoku(ref List<List<int>> matrix, List<int> result) //将01矩阵转换为数独（也可以static）
    {
        List<int> solution = new List<int>();
        for (int i = 1; i <= 81; i++) solution.Add(0);
        for (int ix = 0; ix < 81; ++ix)
        {
            List<int> current = matrix[result[ix] - 1];
            int pos = 0, val = 0;
            for (int jx = 0; jx < 81; ++jx)
            {
                if (current[jx] == 1)
                    break;
                ++pos;
            }
            for (int kx = 81; kx < 162; ++kx)
            {
                if (current[kx] == 1)
                    break;
                ++val;
            }
            solution[pos] = val % 9 + 1;
        }
        return solution;
    }

    public List<int> getResult() { return result; }//返回单个结果

}
