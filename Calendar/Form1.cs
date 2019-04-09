using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{

    public delegate void MyEventHandler();
    public delegate void MyEventHandlerArgs(object o);

    public partial class Form1 : Form
    {
      
        // calendarContentPanel中当前选中日期
        private DateTime currentSelectedDate = DateTime.Today;

        // 当前正在编辑的文本框对应的日期
        private static DateTime currentTextEditingDate = DateTime.Today;

        //记录鼠标移动时上一个位置的日期，减少界面的闪烁
        private int lastDay = -1;

        //鼠标移动的位置
        private int drawPlusInRow = -1;
        private int drawPlusInColoum = -1;

        //鼠标点击的位置
        private int clickInRow = -1;
        private int clickInColoum = -1;

        public Form1()
        {
            // 确保窗口在屏幕中央显示
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        // 当前显示的月份改变事件，在MainForm和DynamicApiQueryQueue中被处理
        public static event MyEventHandlerArgs CurrentShowingMonthChanged;

        // 当前指定的日期改变事件，在MainForm中被处理
        public static event MyEventHandler CurrentSelectedDateChanged;


        private static void OnCurrentShowingMonthChanged(DateMonth dateMonth)
        {
            CurrentShowingMonthChanged?.Invoke(dateMonth);
        }

        private static void OnCurrentSelectedDateChanged()
        {
            CurrentSelectedDateChanged?.Invoke();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Console.WriteLine("MainForm Shown!");


            // 事件布线
            Form1.CurrentShowingMonthChanged += new MyEventHandlerArgs(CurrentShowingMonthChangedHandler);
            Form1.CurrentSelectedDateChanged += new MyEventHandler(CurrentSelectedDateChangedHandler);

            // 初始化NotepadStorage
            NotepadStorage.Initialize();

           
            this.Visible = true;
 
        }

        // CurrentShowingMonthChanged事件处理程序
        private void CurrentShowingMonthChangedHandler(object o)
        {
            DateMonth dateMonth = o as DateMonth;
            if (dateMonth == null)
                throw new ArgumentException();

            currentSelectedDate = new DateTime(dateMonth.Year, dateMonth.Month, currentSelectedDate.Day);

            OnCurrentSelectedDateChanged();
        }

        // CurrentSelectedDateChanged事件处理程序
        private void CurrentSelectedDateChangedHandler()
        {
            // 全部重绘
            dateSelectPanel.Invalidate();
            calendarContentPanel.Invalidate();
            lunarInfoPanel.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.monthCalendar1.Visible = false;
            this.noteTextBox.Visible = false;
            this.deleteButton.Visible = false;
            this.updataButton.Visible = false;


            // 使主窗口接受热键
            this.KeyPreview = true;

        }

        private void calendarContentPanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            // 判断鼠标点击了哪一天
            clickInRow = (int)(e.Location.Y / ((float)calendarContentPanel.Height / 6));
            clickInColoum = (int)(e.Location.X / ((float)calendarContentPanel.Width / 7));

            int[,] daysInPanel = GetDaysInPanel();

            // 点击的单元格有元素存在
            if (daysInPanel[clickInRow, clickInColoum] != 0) 
            {
                // 重置当前选中的日期
                currentSelectedDate = new DateTime(currentSelectedDate.Year, currentSelectedDate.Month, daysInPanel[clickInRow, clickInColoum]);
                OnCurrentSelectedDateChanged();
            }

            // 判断鼠标是否点击了日期框右上角的加号（三角形）
            PointF[] topRightTrianglePoints = {
                   new Point(40, 0),
                   new Point(62, 0),
                   new Point(62, 20)
                };

            for (int i = 0; i < 3; i++)
            {
                topRightTrianglePoints[i].X += (float)calendarContentPanel.ClientRectangle.Width / 7 * drawPlusInColoum;
                topRightTrianglePoints[i].Y += (float)calendarContentPanel.ClientRectangle.Height / 6 * drawPlusInRow;
            }
            GraphicsPath topRightTrianglePath = new GraphicsPath();
            topRightTrianglePath.AddPolygon(topRightTrianglePoints);
            Region topRightTriangleRegion = new Region(topRightTrianglePath);

            // 取得当前在编辑的文本所对应日历中的日期格子
            currentTextEditingDate = new DateTime(
                currentSelectedDate.Year, currentSelectedDate.Month, daysInPanel[drawPlusInRow, drawPlusInColoum]);

           
            // 点击了加号,并且文本没有数据
            if (topRightTriangleRegion.IsVisible(e.Location) && noteTextBox.Text.TrimEnd() == "")
            {
                updataButton.Visible = true;
                deleteButton.Visible = true;
                noteTextBox.Visible = true;
                noteLabel.Visible = false;
                // 文本框取得焦点
                noteTextBox.Focus();
            }
            // 点击了加号,文本有数据,表示添加文本
            else if (topRightTriangleRegion.IsVisible(e.Location) && noteTextBox.Text.TrimEnd() != "") {
                if (!NotepadHashtable.Exists(currentTextEditingDate))
                {
                    NotepadHashtable.AddText(currentTextEditingDate, noteTextBox.Text);
                    MessageBox.Show("添加成功");
                }
                else {
                    NotepadHashtable.UpdateText(currentTextEditingDate, noteTextBox.Text);
                    MessageBox.Show("已添加");
                }
            }
            // 检查NotepadHashtable类中当前日期是否有数据，如有则显示
            else if (NotepadHashtable.Exists(currentTextEditingDate))
            {

                noteTextBox.Text = NotepadHashtable.GetText(currentTextEditingDate);

                updataButton.Visible = true;
                deleteButton.Visible = true;
                noteTextBox.Visible = true;
                noteLabel.Visible = false;

                // 文本框取得焦点
                noteTextBox.Focus();
            }
            else
            {
                updataButton.Visible = false;
                deleteButton.Visible = false;
                noteTextBox.Visible = false;
                noteLabel.Visible = true;
                noteTextBox.Text = "";

                this.Focus();
            }

            // 重绘
            noteTextBox.Invalidate();
        }

        private void dateSelectPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // 仅当按下左键
            if (e.Button != MouseButtons.Left)
                return;

            // 左侧三角形按钮路径
            PointF[] leftTrianglePoints = {
                new PointF(11, 19),
                new PointF(24, 10),
                new PointF(24, 28)
            };

            GraphicsPath leftTrianglePath = new GraphicsPath();
            leftTrianglePath.AddPolygon(leftTrianglePoints);
            Region leftTriangleRegion = new Region(leftTrianglePath);

            // 中部三角形按钮路径
            PointF[] centralTrianglePoints = {
                new PointF(dateLabel.Location.X + dateLabel.Size.Width + 10, 12),
                new PointF(dateLabel.Location.X + dateLabel.Size.Width + 28, 12),
                new PointF(dateLabel.Location.X + dateLabel.Size.Width + 19, 24) };

            GraphicsPath centralTrianglePath = new GraphicsPath();
            centralTrianglePath.AddPolygon(centralTrianglePoints);
            Region centralTriangleRegion = new Region(centralTrianglePath);

            // 右侧三角形按钮路径
            PointF[] rightTrianglePoints = {
                new PointF(dateSelectPanel.Width - 11, dateSelectPanel.Height - 19 + 3),
                new PointF(dateSelectPanel.Width - 24, dateSelectPanel.Height - 28 + 3),
                new PointF(dateSelectPanel.Width - 24, dateSelectPanel.Height - 10 + 3)
            };

            GraphicsPath rightTrianglePath = new GraphicsPath();
            rightTrianglePath.AddPolygon(rightTrianglePoints);
            Region rightTriangleRegion = new Region(rightTrianglePath);


            // 判断按下了哪个按钮并处理事件
            // 按下左侧三角形或右侧三角形后，需要将currentSelectedDate向前或向后移一个月
            if (leftTriangleRegion.IsVisible(e.X, e.Y))
            {
                currentSelectedDate = currentSelectedDate.AddMonths(-1);
                OnCurrentShowingMonthChanged(new DateMonth(currentSelectedDate));
            }
            else if (centralTriangleRegion.IsVisible(e.X, e.Y))
            {
                dateLabel_MouseClick(sender, e);
            }
            else if (rightTriangleRegion.IsVisible(e.X, e.Y))
            {
                currentSelectedDate = currentSelectedDate.AddMonths(1);
                OnCurrentShowingMonthChanged(new DateMonth(currentSelectedDate));
            }
        }

        private void dateLabel_MouseClick(object sender, MouseEventArgs e)
        {
            // 点击左键，且当前是月历模式才响应
            if (e.Button != MouseButtons.Left)
                return;

            if (!monthCalendar1.Visible)
            {
                // 导航到当前日历选中的日期
                monthCalendar1.SelectionStart = currentSelectedDate;
                monthCalendar1.Visible = true;
            }
            else
            {
                monthCalendar1.Visible = false;
                // 主窗口重新获取焦点，防止按键无响应或响应错误
                this.Focus();
            }
        }

        private void calendarContentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // 当且仅当鼠标在日历面板范围内才响应
            if (e.X > 0 && e.X < calendarContentPanel.Width && e.Y > 0 && e.Y < calendarContentPanel.Height)
            {
                int[,] daysInPanel = GetDaysInPanel();

                // 判断鼠标移到了哪一天
                int row = (int)(e.Y / ((float)calendarContentPanel.Height / 6));
                int coloum = (int)(e.X / ((float)calendarContentPanel.Width / 7));

                // 在相应位置画加号
                if (daysInPanel[row, coloum] != 0)
                {
                    drawPlusInRow = row;
                    drawPlusInColoum = coloum;

                    if (lastDay == daysInPanel[row, coloum]) return;
                    else lastDay = -1;

                    if (lastDay == -1) {
                        lastDay = daysInPanel[row, coloum];
                        calendarContentPanel.Invalidate();
                    }
                  
                }
                else
                {
                    // 重置Plus的位置
                    drawPlusInRow = -1;
                    drawPlusInColoum = -1;
                }
            }
        }

        private void calendarContentPanel_MouseLeave(object sender, EventArgs e)
        {
            // 重置Plus的位置
            drawPlusInRow = -1;
            drawPlusInColoum = -1;
        }

        private void dateSelectPanel_Paint(object sender, PaintEventArgs e)
        {
            // 写入日期文本
            //string dateText = DateTime.Now; // 2016/8/18 11:49:42
            DateTime date = currentSelectedDate;
            string dateText = date.Year.ToString().PadLeft(4, '0') + "年"
                + date.Month.ToString().PadLeft(2, '0') + "月"
                + date.Day.ToString().PadLeft(2, '0') + "日";
            dateLabel.Text = dateText;

            Graphics g = e.Graphics;

            //如果在月历模式下，绘制按钮
              // 绘制中部三角形按钮
                PointF[] centralTrianglePoints = {
                new PointF(dateLabel.Location.X + dateLabel.Size.Width + 10, 12),
                new PointF(dateLabel.Location.X + dateLabel.Size.Width + 28, 12),
                new PointF(dateLabel.Location.X + dateLabel.Size.Width + 19, 24) };
                g.FillPolygon(Brushes.White, centralTrianglePoints);

                // 绘制左侧三角形按钮
                PointF[] leftTrianglePoints = {
                new PointF(11, 19),
                new PointF(24, 10),
                new PointF(24, 28)
                };
                g.FillPolygon(Brushes.White, leftTrianglePoints);

                // 绘制右侧三角形按钮
                PointF[] rightTrianglePoints = {
                new PointF(dateSelectPanel.Width - 11, dateSelectPanel.Height - 19 + 3),
                new PointF(dateSelectPanel.Width - 24, dateSelectPanel.Height - 28 + 3),
                new PointF(dateSelectPanel.Width - 24, dateSelectPanel.Height - 10 + 3)
                };
                g.FillPolygon(Brushes.White, rightTrianglePoints);
           
        }

        private void weekPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 绘制“周日”~“周六”文本
            const float translationDistance = 27;
            const float fixedLengthwaysDistance = 4; // previously = 8
            string[] sampleText = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            float[] separatePosition = new float[7];
            Font font = new Font("微软雅黑", 10);
            for (int i = 0; i < 7; i++)
            {
                separatePosition[i] = (float)calendarContentPanel.Width / 7 * i;
                g.DrawString(sampleText[i],
                            font,
                            Brushes.White,
                            new PointF(translationDistance + separatePosition[i],
                                        fixedLengthwaysDistance));
            }
        }

        private void calendarContentPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen frameworkPen = new Pen(Color.FromArgb(176, 175, 175), 2);
            // 列*7
            float currentSeparateWidth;
            for (int i = 0; i < 6; i++)
            {
                currentSeparateWidth = (float)calendarContentPanel.Width / 7 * (i + 1);
                g.DrawLine(frameworkPen,
                            new PointF(currentSeparateWidth, 0),
                            new PointF(currentSeparateWidth, (float)calendarContentPanel.Height));
            }
            //行*6
            float currentSeparateHeight;
            for (int i = 0; i < 5; i++)
            {
                currentSeparateHeight = (float)calendarContentPanel.Height / 6 * (i + 1);
                g.DrawLine(frameworkPen,
                            new PointF(0, currentSeparateHeight),
                            new PointF((float)calendarContentPanel.Width, currentSeparateHeight));
            }

            // 加深今天的背景颜色
            // 仅当当前显示本月时绘制
            DateTime today = DateTime.Today;
            if (currentSelectedDate.Year == today.Year && currentSelectedDate.Month == today.Month)
            {
                int todayRow;
                int todayColumn;
                // 得到今天在日历中的位置
                GetDayPositionInCalendarContentPanel(out todayRow, out todayColumn, DateTime.Today); 
                SolidBrush todayBackgroundBrush = new SolidBrush(Color.FromArgb(50, 50, 50));
                RectangleF todayRect = new RectangleF(
                    todayColumn * ((float)calendarContentPanel.Width / 7) + 1,
                    todayRow * ((float)calendarContentPanel.Height / 6) + 1,
                    (float)calendarContentPanel.Width / 7 - 2,
                    (float)calendarContentPanel.Height / 6 - 2);
                g.FillRectangle(todayBackgroundBrush, todayRect);
            }

            //加深被选择的日期
            int selectedRow;
            int selectedColumn;
            GetDayPositionInCalendarContentPanel(out selectedRow, out selectedColumn);
            SolidBrush selectedBackgroundBrush = new SolidBrush(Color.FromArgb(50, 100, 100));
            RectangleF selectedRect = new RectangleF(
                   selectedColumn * ((float)calendarContentPanel.Width / 7) + 1,
                   selectedRow * ((float)calendarContentPanel.Height / 6) + 1,
                   (float)calendarContentPanel.Width / 7 - 2,
                   (float)calendarContentPanel.Height / 6 - 2);
            g.FillRectangle(selectedBackgroundBrush, selectedRect);

            // 绘制日期数字
            // 该月的天数
            int daysOfMonth = DateTime.DaysInMonth(currentSelectedDate.Year, currentSelectedDate.Month); 

            PointF[] numberPositions = new PointF[daysOfMonth];
            // 绘制时，日历表格中的当前行数
            int rowOfNumbers = 0;
            // 表示当前行的数字数量
            int numbersInCurrentRow = 0;
            // 表示后5行的第一个索引数（day-1），仅需用到后五个元素，首元素恒为0
            int[] firstIndexsInRows = new int[6];

            // 为每个数字增加的横向宽度
            const float fixedNumberDistanceX = 15;
            // 为每个数字增加的竖向高度
            const float fixedNumberDistanceY = 5;
            // 为单个数字（0~9）增加的横向宽度
            const float fixedNumberAdditionalCrosswiseDistanceForSingleFigure = 5; 

            //字体
            Font numberFont = new Font("微软雅黑", 15);
            //普通日期画笔
            SolidBrush numberNormalBrush = new SolidBrush(Color.FromArgb(252, 252, 252));
            //节假日画笔
            SolidBrush numberHolidayBrush = new SolidBrush(Color.FromArgb(238, 119, 0));

            // 该月第一天星期几（0~6）
            int weekOfMonthFirstDay = (int)DateTime.Parse(
                currentSelectedDate.Year.ToString() + "/" + currentSelectedDate.Month.ToString() + "/1"
                ).DayOfWeek;

            // 根据本月的第一天是星期几来绘制第一个数字
            firstIndexsInRows[0] = numbersInCurrentRow = weekOfMonthFirstDay;
            for (int i = 0; i < daysOfMonth; i++)
            {
                if (numbersInCurrentRow >= 7) // 如果该行元素数量超过7个，则换行绘制
                {
                    rowOfNumbers++;
                    numbersInCurrentRow = 0;
                    firstIndexsInRows[rowOfNumbers] = i;
                }
                numberPositions[i].X = numbersInCurrentRow * ((float)calendarContentPanel.Width / 7) + fixedNumberDistanceX;
                numberPositions[i].Y = rowOfNumbers * ((float)calendarContentPanel.Height / 6) + fixedNumberDistanceY;
                if (i < 9) // 为单个数字（0~9）增加横向宽度
                    numberPositions[i].X += fixedNumberAdditionalCrosswiseDistanceForSingleFigure;

                // 判断是否为节假日并用不同画刷绘制
                int weekOfCurrentDate = (int)DateTime.Parse(
                    currentSelectedDate.Year.ToString() + "/" + currentSelectedDate.Month.ToString() + "/" + (i + 1).ToString()
                    ).DayOfWeek;
                if (weekOfCurrentDate == 0 || weekOfCurrentDate == 6)
                    g.DrawString((i + 1).ToString(), numberFont, numberHolidayBrush, numberPositions[i]);
                else
                    g.DrawString((i + 1).ToString(), numberFont, numberNormalBrush, numberPositions[i]);

                // 增加本行的数字数量
                numbersInCurrentRow++;
            }   

            // 在已有记事本数据的日期右上角绘制提示标记
            DateTime currentLoopDate = new DateTime(currentSelectedDate.Year, currentSelectedDate.Month, 1);

            int[,] daysInPanel = GetDaysInPanel();

            for (int i = 0; i < daysOfMonth; i++)
            {
                currentLoopDate = currentLoopDate.AddDays(1);

                if (NotepadHashtable.Exists(currentLoopDate))
                {
                    for (int j = 0; j < 6; j++)
                    {
                        for (int k = 0; k < 7; k++)
                        {
                            if (daysInPanel[j, k] == currentLoopDate.Day)
                            {
                                // 绘制三角形
                                PointF[] topRightTrianglePoints = {
                                     new Point(40, 0),
                                     new Point(62, 0),
                                     new Point(62, 20)
                                };

                                for (int m = 0; m < 3; m++)
                                {
                                    topRightTrianglePoints[m].X += (float)calendarContentPanel.ClientRectangle.Width / 7 * k;
                                    topRightTrianglePoints[m].Y += (float)calendarContentPanel.ClientRectangle.Height / 6 * j;
                                }

                                SolidBrush topRightMemoryTriangleBrush = new SolidBrush(Color.FromArgb(255, 189, 140)); // 255, 255, 140
                                g.FillPolygon(topRightMemoryTriangleBrush, topRightTrianglePoints);
                            }
                        }
                    }
                }
            }

            // 如果鼠标在日历画板上停留，绘制加号和三角形背景
            if (drawPlusInRow != -1 && drawPlusInColoum != -1)
            {
                // 绘制三角形
                PointF[] topRightTrianglePoints = {
                    new Point(40, 0),
                    new Point(62, 0),
                    new Point(62, 20)
                };
                for (int i = 0; i < 3; i++)
                {
                    topRightTrianglePoints[i].X += (float)calendarContentPanel.ClientRectangle.Width / 7 * drawPlusInColoum;
                    topRightTrianglePoints[i].Y += (float)calendarContentPanel.ClientRectangle.Height / 6 * drawPlusInRow;
                }

                SolidBrush topRightMouseMovingBrush = new SolidBrush(Color.FromArgb(255, 189, 140));
                g.FillPolygon(topRightMouseMovingBrush, topRightTrianglePoints);

                // 绘制加号
                const float fixedPlusCrosswiseDistance = 50;
                const float fixedPlusLengthwaysDistance = 1;
                const float fixedPlusWidth = 8;
                const float fixedPlusHeight = 8;

                PointF plusHorizontalStartPoint = new PointF(
                    (float)calendarContentPanel.ClientRectangle.Width / 7 * drawPlusInColoum + fixedPlusCrosswiseDistance,
                    (float)calendarContentPanel.ClientRectangle.Height / 6 * drawPlusInRow + fixedPlusLengthwaysDistance + fixedPlusHeight / 2
                    );
                PointF plusHorizontalEndPoint = new PointF(
                    (float)plusHorizontalStartPoint.X + fixedPlusWidth,
                    (float)plusHorizontalStartPoint.Y
                    );
                PointF plusVerticalStartPoint = new PointF(
                    (float)calendarContentPanel.ClientRectangle.Width / 7 * drawPlusInColoum + fixedPlusCrosswiseDistance + fixedPlusWidth / 2,
                    (float)calendarContentPanel.ClientRectangle.Height / 6 * drawPlusInRow + fixedPlusLengthwaysDistance
                    );
                PointF plusVerticalEndPoint = new PointF(
                    (float)plusVerticalStartPoint.X,
                    (float)plusVerticalStartPoint.Y + fixedPlusHeight
                    );

                g.DrawLine(Pens.White, plusHorizontalStartPoint, plusHorizontalEndPoint);
                g.DrawLine(Pens.White, plusVerticalStartPoint, plusVerticalEndPoint);
            }
        }

        private void lunarInfoPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            OnCurrentSelectedDateChanged();
            OnCurrentShowingMonthChanged(new DateMonth(currentSelectedDate.Year, currentSelectedDate.Month));
        }

        /**
         * 得到今天在日历中的位置
         */
        private void GetDayPositionInCalendarContentPanel(out int dayInTableRow, out int dayInTableColumn, DateTime? date = null)
        {
            DateTime trueDate = (date.HasValue) ? date.Value : currentSelectedDate; // 取得真实日期

            int weekOfMonthFirstDay = (int)DateTime.Parse(
                trueDate.Year.ToString() + "/" + trueDate.Month.ToString() + "/1"
                ).DayOfWeek; // 该月第一天星期几（0~6）

            // 计算date（默认为currentSelectedDate）在表格中的位置

            if (trueDate.Day <= 7 - weekOfMonthFirstDay)
            {
                dayInTableRow = 0;
                dayInTableColumn = trueDate.Day - 1 + weekOfMonthFirstDay;
            }
            else
            {
                dayInTableRow = (trueDate.Day + weekOfMonthFirstDay - 1) / 7;
                dayInTableColumn = ((trueDate.Day - 1) % 7 + weekOfMonthFirstDay) % 7;
            }
        }  
      

        /**
         * 取得表中日期天数的数组
         */
        private int[,] GetDaysInPanel()
        {
            int[,] daysInPanel = new int[6, 7]; // 存放表格中每个单元格显示的数字，无数字为0

            //DateTime date = DateTime.Today;
            int weekOfMonthFirstDay = (int)DateTime.Parse(
                currentSelectedDate.Year.ToString() + "/" + currentSelectedDate.Month.ToString() + "/1"
                ).DayOfWeek; // 该月第一天星期几（0~6）
            int daysOfMonth = DateTime.DaysInMonth(currentSelectedDate.Year, currentSelectedDate.Month); // 该月的天数

            int dayCount = 1;
            for (int i = 0; i < 6; i++)
            {
                for (int j = (i == 0 ? weekOfMonthFirstDay : 0); j < 7; j++)
                {
                    if (dayCount <= daysOfMonth)
                    {
                        daysInPanel[i, j] = dayCount++; // 将对应的日期存入daysInPanel数组
                    }
                    else
                        break;
                }
            }
            return daysInPanel;
        }

        private void updataButton_Click(object sender, EventArgs e)
        {
            int[,] daysInPanel = GetDaysInPanel();
            if (daysInPanel[clickInRow, clickInColoum] != 0)
            {
                currentTextEditingDate = new DateTime(
                 currentSelectedDate.Year, currentSelectedDate.Month, daysInPanel[clickInRow, clickInColoum]);

                // 更新数据
                if (NotepadHashtable.Exists(currentTextEditingDate) && noteTextBox.Text.TrimEnd() != "")
                {
                    NotepadHashtable.UpdateText(currentTextEditingDate, noteTextBox.Text);
                    MessageBox.Show("更新成功");
                }
                else
                {
                    MessageBox.Show("更新失败");
                }
            }

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
          
            int[,] daysInPanel = GetDaysInPanel();
            if (daysInPanel[clickInRow, clickInColoum] != 0) {
                currentTextEditingDate = new DateTime(
              currentSelectedDate.Year, currentSelectedDate.Month, daysInPanel[clickInRow, clickInColoum]);

                // 删除哈希表中的数据
                if (NotepadHashtable.Exists(currentTextEditingDate))
                {
                    NotepadHashtable.RemoveText(currentTextEditingDate);

                    noteTextBox.Text = "";

                    updataButton.Visible = false;
                    deleteButton.Visible = false;
                    noteTextBox.Visible = false;
                    noteLabel.Visible = true;


                    MessageBox.Show("删除成功");
                }
                else
                {
                    MessageBox.Show("删除失败");

                }
            }
          
        }
    }
}
