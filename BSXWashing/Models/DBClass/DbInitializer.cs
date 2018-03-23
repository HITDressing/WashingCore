using System.Linq;

namespace BSXWashing.Models.DBClass
{
    public class DbInitializer
    {
        public static void Initialize(WashingContext context)
        {
            context.Database.EnsureCreated();

            if (context.AccountModels.Any())
            {
                return;   // DB has been seeded
            }

            var accounts = new AccountModel[]
            {
                new AccountModel
                {
                    AccountName = "admin",
                    Balance =0,Address="管理员的地址",
                    Level = EnumClass.EnumAccountLevel.E类,
                    Category = EnumClass.EnumAccountCategory.预留,
                    Type = EnumClass.EnumAccountType.超级管理员,
                    StoreName = "管理员的商店",
                    MobileNumber="13312345678",
                    Password="12345678"
                },
                new AccountModel
                {
                    AccountName = "ware",
                    Balance =0,Address="仓库保管员的地址",
                    Level = EnumClass.EnumAccountLevel.E类,
                    Category = EnumClass.EnumAccountCategory.预留,
                    Type = EnumClass.EnumAccountType.仓库保管员,
                    StoreName = "仓库保管员的商店",
                    MobileNumber="13312345678",
                    Password="12345678"
                },
                new AccountModel
                {
                    AccountName = "finance",
                    Balance =0,Address="财务负责人的地址",
                    Level = EnumClass.EnumAccountLevel.E类,
                    Category = EnumClass.EnumAccountCategory.预留,
                    Type = EnumClass.EnumAccountType.财务负责人,
                    StoreName = "财务负责人的商店",
                    MobileNumber="13312345678",
                    Password="12345678"
                },
                new AccountModel
                {
                    AccountName = "tran1",
                    Balance =0,Address="配送专员的地址",
                    Level = EnumClass.EnumAccountLevel.E类,
                    Category = EnumClass.EnumAccountCategory.预留,
                    Type = EnumClass.EnumAccountType.配送专员,
                    StoreName = "配送专员的商店",
                    MobileNumber="15655898758",
                    Password="12345678"
                },
                new AccountModel
                {
                    AccountName = "user1",
                    Balance =0,Address="客户的地址",
                    Level = EnumClass.EnumAccountLevel.E类,
                    Category = EnumClass.EnumAccountCategory.预留,
                    Type = EnumClass.EnumAccountType.客户,
                    StoreName = "客户的商店",
                    MobileNumber="18925467852",
                    Password="12345678"
                }
            };

            var items = new ItemModel[]
            {
                new ItemModel{ ItemName = "床单 1.2M",ItemValue = 1.2},
                new ItemModel{ ItemName = "床单 1.5M",ItemValue = 1.5},
                new ItemModel{ ItemName = "床单 1.8M",ItemValue = 1.8},
                new ItemModel{ ItemName = "被套 1.2M",ItemValue = 1.2},
                new ItemModel{ ItemName = "被套 1.5M",ItemValue = 1.5},
                new ItemModel{ ItemName = "被套 1.8M",ItemValue = 1.8},

                new ItemModel{ ItemName = "枕套",ItemValue = 1.0},
                new ItemModel{ ItemName = "浴巾",ItemValue = 1.0},
                new ItemModel{ ItemName = "地巾",ItemValue = 1.0},
                new ItemModel{ ItemName = "毛巾",ItemValue = 1.0},
                new ItemModel{ ItemName = "方巾",ItemValue = 1.0},
                new ItemModel{ ItemName = "大台布",ItemValue = 3.0},
                new ItemModel{ ItemName = "香巾",ItemValue = 3.0},
                new ItemModel{ ItemName = "小台布",ItemValue = 3.0},

                new ItemModel{ ItemName = "口布",ItemValue = 3.0},
                new ItemModel{ ItemName = "毛毯",ItemValue = 3.0},
                new ItemModel{ ItemName = "桌围裙",ItemValue = 3.0},
                new ItemModel{ ItemName = "厨衣",ItemValue = 3.0},
                new ItemModel{ ItemName = "窗帘",ItemValue = 3.0},
                new ItemModel{ ItemName = "窗帘内胆",ItemValue = 3.0},
                new ItemModel{ ItemName = "浴帘",ItemValue = 3.0},
                new ItemModel{ ItemName = "浴服",ItemValue = 3.0},
                new ItemModel{ ItemName = "椅套",ItemValue = 3.0},
                new ItemModel{ ItemName = "帽子",ItemValue = 3.0},
                new ItemModel{ ItemName = "床裙",ItemValue = 3.0},
                new ItemModel{ ItemName = "缎料工服",ItemValue = 3.0},
                new ItemModel{ ItemName = "唐装",ItemValue = 3.0},
                new ItemModel{ ItemName = "免烫工服",ItemValue = 3.0},
                new ItemModel{ ItemName = "旗袍",ItemValue = 3.0},
                new ItemModel{ ItemName = "西服",ItemValue = 3.0},
                new ItemModel{ ItemName = "领带",ItemValue = 3.0},
                new ItemModel{ ItemName = "已烫工服",ItemValue = 3.0},
                new ItemModel{ ItemName = "沙发套",ItemValue = 3.0},
                new ItemModel{ ItemName = "床罩",ItemValue = 3.0},
                new ItemModel{ ItemName = "抹布",ItemValue = 3.0},
                new ItemModel{ ItemName = "保护垫",ItemValue = 3.0},
                new ItemModel{ ItemName = "地毯清洗",ItemValue = 3.0},
                new ItemModel{ ItemName = "足浴窄床单",ItemValue = 3.0},
                new ItemModel{ ItemName = "预留",ItemValue = 3.0},
                new ItemModel{ ItemName = "预留",ItemValue = 3.0}
            };

            context.ItemModels.AddRange(items);
            context.AccountModels.AddRange(accounts);
            context.SaveChanges();
        }
    }
}
