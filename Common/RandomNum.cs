using System;

namespace Common
{
    public class RandomNum
    {
        private Random random;

        public RandomNum()
        {
            long tick = DateTime.Now.Ticks;
            random = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
        }

        /// <summary>
        /// 构造函数设置种子，传0为用GUID的哈希值作为种子
        /// </summary>
        public RandomNum(int seed)
        {
            if (seed == 0)
            {
                seed = Guid.NewGuid().GetHashCode();
            }
            random = new Random(seed);
        }

        /// <summary>
        /// 获取伪随机数生成器
        /// </summary>
        /// <returns></returns>
        public Random GetRandom()
        {
            return random;
        }

        /// <summary>
        /// 获取m个n范围内【[0,n)】的不重复随机数
        /// </summary>
        /// <param name="m">多少个</param>
        /// <param name="n">上限值（不包含此值）</param>
        /// <returns></returns>
        public int[] GetDifferentInt(int m, int n)
        {
            if (m < 1 || m > n)
            {
                throw new ArgumentException("参数有误，m需大于等于1且小于等于n");
            }
            var arr = new int[n];
            for (int i = 0; i < n; i++)
            {
                arr[i] = i;
            }
            for (int i = 0; i < n; i++)
            {
                var r = random.Next(i, n - 1);
                int tmp = arr[i];
                arr[i] = arr[r];
                arr[r] = tmp;
            }

            var res = new int[m];
            Array.Copy(arr, res, m);

            return res;
        }
    }
}
