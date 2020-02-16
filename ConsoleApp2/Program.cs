using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static int __index = 0;
        static List<int> __pickList = new List<int>();

        static int __initData = -1;
        static List<int> __selectData = new List<int>(); //{ 4, 6, 8, 5 };

        static void Main(string[] args)
        {
            Console.WriteLine("=======================================================");
            Console.WriteLine("=== [현명한 상인의 도전] 5번째문제 풀기 by 솔티멜론 ===");
            Console.WriteLine("=======================================================");


            Console.WriteLine("초기 시간을 입력해주세요.");
            string initTime_str = Console.ReadLine();
            __initData = Convert.ToInt32(initTime_str);


            Console.WriteLine("1번째 - 선택 가능한 시간을 입력해주세요.");
            string st_1_str = Console.ReadLine();
            Console.WriteLine("2번째 - 선택 가능한 시간을 입력해주세요.");
            string st_2_str = Console.ReadLine();
            Console.WriteLine("3번째 - 선택 가능한 시간을 입력해주세요.");
            string st_3_str = Console.ReadLine();
            Console.WriteLine("4번째 - 선택 가능한 시간을 입력해주세요.");
            string st_4_str = Console.ReadLine();

            __selectData.Add(Convert.ToInt32(st_1_str));
            __selectData.Add(Convert.ToInt32(st_2_str));
            __selectData.Add(Convert.ToInt32(st_3_str));
            __selectData.Add(Convert.ToInt32(st_4_str));

            __index = 0;

            // 여기를 없애야함. TODO
            // 보정 정답은 무조건 5자리가 되어야 해서
            // selectData에서 마지막값을 한번더 추가해줌.
            __selectData.Add(__selectData.Last<int>());

            while (true)
            {
                int sumdata = GetSumData(__selectData, ref __index);
                if (sumdata == -1)
                {
                    // 더이상 추출이 불가능하면 종료.
                    ImpossibleEND();
                    break;
                }
                int checkdata = __initData + sumdata;

                if (checkdata % 12 == 0) // 더한수가 12의 배수 인지 확인.
                {
                    NormalEND();
                    break;
                }

                // 12의 배수가 아니라면 다시 시도.
                __index++;
            }

        }

        private static int GetSumData(List<int> data, ref int index)
        {
            int max_count = data.Count;

            int index_max = max_count * max_count;

            if (index >= index_max)
            {
                // 여기로 타면 풀 수 없는 문제임.
                return -1;
            }
            
            List<int> getNumList = new List<int>();

            for (int i = 0; i < max_count; i++)
            {
                // 루프를 돌때마다 자리를 추출
                int get_num = 0;
                int num_sum = 0;
                
                if (i == 0)
                {
                    // 1번쨰 자리는 3으로 %하면 나머지.
                    get_num = index % max_count;
                }
                else
                {
                    // 2번째 자리는 1의 자리를 빼고 3으로 나누면 몫
                    // 3번째 자리는 1,2자리빼고 3*3으로 나누면 몫
                    int temp = index;
                    temp -= num_sum;
                    int pow = (int)Math.Pow(max_count, i);
                    temp = temp / pow;
                    get_num = temp;
                }

                if (get_num >= max_count)
                {
                    RaiseError("get_num is big get_num:"+get_num);
                    return -1;
                }

                // TODO get_num 저장하기
                getNumList.Add(get_num);

                num_sum += get_num;
            }

            // index 계산하기
            index = CalculateIndex(getNumList);


            // getNumList를 더해서 리턴
            int ret = SumPickData(data, getNumList);

            return ret;
        }

        private static int CalculateIndex(List<int> list)
        {
            // 0,0,0  // index (3*3*0)+(3*0)+0 = 0  (0+0+0)
            // 0,0,1  // index (3*3*0)+(3*0)+1 = 1  (0+0+1)
            // 0,0,2  // index (3*3*0)+(3*0)+2 = 2  (0+0+2)

            // 0,1,0  // index (3*3*0)+(3*1)+0 = 3  (0+3+0)
            // 0,1,1  // index (3*3*0)+(3*1)+1 = 4  (0+3+1)
            // 0,1,2  // index (3*3*0)+(3*1)+2 = 5  (0+3+2)

            // 0,2,0  // index (3*3*0)+(3*2)+0 = 6  (0+6+0)
            // 0,2,1  // index (3*3*0)+(3*2)+1 = 7  (0+6+1)
            // 0,2,2  // index (3*3*0)+(3*2)+2 = 8  (0+6+2)

            // 1,0,0  // index (3*3*1)+(3*0)+0 = 9  (9+0+0)
            // 1,0,1  // index (3*3*1)+(3*0)+1 = 10 (9+0+1)
            // 1,0,2  // index (3*3*1)+(3*0)+2 = 11 (9+0+2)

            // 1,1,0  // index (3*3*1)+(3*1)+0 = 12 (9+3+0)
            // 1,1,1  // index (3*3*1)+(3*1)+1 = 13 (9+3+1)
            // 1,1,2  // index (3*3*1)+(3*1)+2 = 14 (9+3+2)

            // 1,2,0  // index (3*3*1)+(3*2)+0 = 15 (9+6+0)
            // 1,2,1  // index (3*3*1)+(3*2)+1 = 16 (9+6+1)
            // 1,2,2  // index (3*3*1)+(3*2)+2 = 17 (9+6+2)

            // 2,0,0  // index (3*3*2)+(3*0)+0 = 18 (18+0+0)
            // 2,0,1  // index (3*3*2)+(3*0)+1 = 19 (18+0+1)
            // 2,0,2  // index (3*3*2)+(3*0)+2 = 20 (18+0+2)

            // 2,1,0  // index (3*3*2)+(3*1)+0 = 21
            // 2,1,1  // index (3*3*2)+(3*1)+1 = 22
            // 2,1,2  // index (3*3*2)+(3*1)+2 = 23

            // 2,2,0  // index (3*3*2)+(3*2)+0 = 24
            // 2,2,1  // index (3*3*2)+(3*2)+1 = 25
            // 2,2,2  // index (3*3*2)+(3*2)+2 = 26

            int max_count = list.Count;

            int ret = 0;
            for (int i = 0; i < max_count; i++)
            {
                int currentNumber = list[i];

                int n = (int)Math.Pow(max_count, i) * currentNumber;

                ret += n;
            }

            return ret;
        }

        private static int SumPickData(List<int> data, List<int> getList)
        {
            int sum = 0;
            __pickList.Clear();

            for (int i = 0; i < getList.Count; i++)
            {
                int getNum = getList[i];
                int pickNum = data[getNum];
                __pickList.Add(pickNum);
                sum += pickNum;
            }

            return sum;
        }

        private static void ImpossibleEND()
        {
            Console.WriteLine("===[풀 수 없는 문제입니다.]===");
            Console.WriteLine("__index=" + __index);
            Console.WriteLine("===입력 값이 올바른지 확인해 주세요.");

            Console.ReadKey();

            Environment.Exit(0);
        }

        private static void NormalEND()
        {
            Console.WriteLine("========================[END]==============================");
            Console.WriteLine("__index=" + __index);
            Console.WriteLine("__initData=" + __initData);

            DisplaySum(__initData, __pickList);
            DisplayPickList(__pickList);
            
            Console.ReadKey();

            Environment.Exit(0);
        }

        private static void DisplayPickList(List<int> list)
        {
            Console.WriteLine("===[정답]");

            foreach (var num in list)
            {
                Console.Write("" + num);
                Console.Write(" ");
            }

            Console.WriteLine("");
        }

        private static void DisplaySum(int initData, List<int> pickList)
        {
            Console.WriteLine("===[정답 검증]");

            int sum = 0;
            sum += initData;

            foreach (var num in pickList)
            {
                sum += num;
            }

            Console.WriteLine("전체합은:"+sum);
        }

        private static void RaiseError(string msg)
        {
            Console.WriteLine("===[에러발생] msg:"+msg);

            Console.WriteLine("__index=" + __index);

            Console.ReadKey();

            Environment.Exit(0);
        }
    }
}
