using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ors.Core;
using Ors.Core.Security;
using Ors.Framework.Complilation;
using Ors.Core.Data;

namespace Ors.Test
{
    public class User:AbstractModel
    {
        public int? Level { get; set; }
    }
    public class UserQuery:AbstractQuery<User>
    {
        public int? Level { get; set; }
    }
    public class TestModel:AbstractModel
    {
        public int? Index { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
    public class TestQuery:AbstractQuery<TestModel>
    {
        public int? Index { get; set; }
        public string NamePattern { get; set; }
        public string[] NameList { get; set; }
        public int[] IndexList { get; set; }
        public Range<int> IndexRange { get; set; }
        public UserQuery UserQuery { get; set; }
    }
    public class FrameworkTest
    {
        public void Test()
        {
            
            var source = new List<TestModel>()
                {
                    new TestModel()
                        {
                            Index = 3,
                            Name = "abcdefg",
                            User = new User(){Level = 2}
                        },
                    new TestModel()
                        {
                            Index = 1,
                            Name = "hijklmn",
                            User = new User(){Level = 3}
                        },
                    new TestModel()
                        {
                            Index = 2,
                            Name = "uvwxyz",
                            User = new User(){Level = 1}
                        }
                }.AsQueryable();

            var whereMethod =
                typeof (Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(mi => mi.Name == "Where" &&
                                                                                                               mi.GetParameters()[1].ParameterType.GetGenericArguments().Count() == 2);
            whereMethod = whereMethod.MakeGenericMethod(new Type[] {typeof (TestModel)});
            Func<TestModel, bool> expr = model => model.Index == 2;
            var result = whereMethod.Invoke(source, new object[] {source, expr}) as IEnumerable<TestModel>;
            var t = Expression.Parameter(typeof (TestModel), "t");
            var predictExpr = Expression.Lambda(
                        Expression.Equal(Expression.Constant(2,typeof(int?)), Expression.Property(t, typeof(TestModel).GetProperty("Index"))),
                        t
                        );
            result = whereMethod.Invoke(source, new object[] {source, predictExpr.Compile()}) as IEnumerable<TestModel>;

            var query = new TestQuery() 
            { 
                OrderField = "Index",UserQuery = new UserQuery(){Level = 1},
                Take = 1,Skip = 0
            };
            var func = Compiler.Compile<TestModel>(query);
            result = func(source, query).ToList();
            var testobj = new TestModel() {ID = 1, Name = "123", User = new User()};
            
            var list = new List<string>();
            list.Remove("123");
            Console.ReadKey();

        }

        public void TestUtil()
        {
            var str = "111111";
            var hash = str.Hash();
        }
    }
}
