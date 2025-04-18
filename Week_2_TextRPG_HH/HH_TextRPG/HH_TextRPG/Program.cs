using System.Linq;
using System.Collections.Generic;



/* 제출자의 멘션
이게 하면서 점점 스끼리(?)를 하나씩 넣다 보니 중구난방입니다...
특히 인게임에 필요한 데이터들을 넣고 꺼내오는 방법이 제각각이네요. (순서대로 만듦)
1. 아이템 리스트 : 스트링 이차원 배열로 때려박음 -> 총체적 난국이었네요. 하필 얽힌 부분이 많아서 코드가 읽기 많이 불편하실 것 같아요...
2. 유저 : 구조체를 만드는 것까진 좋았는데 유저 정보를 리스트로 안 만들었음. 이것도 이제 와서 고치려니 시간이...
3. 던전 데이터 : 던전 데이터 구조화 + 데이터는 리스트로 저장. 이게 앞에 둘보다는 더 보편적인 방법인 것 같은데, 더 좋은 방법이 있을까요?
*/
namespace HH_TextRPG
{
    internal class Program
    {
        public struct Dungeon_Data()
        {
            public int Required_Def;
            public int Gold_Drop;
            public float Fail_Rate;
            public float Fail_Penelty;
            public string Name;
        }
        // 던전 데이터 구조체화
        public struct User()
        {
            public string Name;
            public int Major;
            public float Strength_Point;
            public float Defence_Point;
            public float Health_Point;
            public int Gold;
            public int Lvl;
            public int Exp;

            public void PrintInfo()
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                Console.WriteLine($"Lv. {Lvl}");
                Console.Write($"{Name}  ");
                Console.WriteLine($"({Enum.GetName(typeof(MajorType), Major)})");
                Console.WriteLine($"공격력 : {Strength_Point:N1} (+ {(Strength_Point- (Lvl-1)*0.5f - 10):N1})");
                Console.WriteLine($"방어력 : {Defence_Point} (+ {Defence_Point-(Lvl-1)-5})");
                Console.WriteLine($"체력 : {Health_Point:N1}");
                Console.WriteLine($"Gold : {Gold} G\n");

                Console.WriteLine("0 : 나가기\n");
                UserInputHandler(0, true);
                Console.Clear();
            }
        }
        //유저 정보 구조체 만들어 보기 (추후 시간 나서 여러 유저 데이터를 만든다면 쓸만하겠지)

        public static void StatApplying()
        {
            int item_DefenceStat = 0;
            int item_AttackStat = 0;

            for (int i = 0; i < Item_List.Length; i++)
            {
                if (Item_List[i][5] == "1" && Item_List[i][6] == "1") // 착용 중이면서 방어구이면
                    item_DefenceStat += Int32.Parse(Item_List[i][2]);
                else if (Item_List[i][5] == "1" && Item_List[i][6] == "2") // 착용 중이면서 무기이면
                    item_AttackStat += Int32.Parse(Item_List[i][2]);
            }

            current_user.Strength_Point = 10 + item_AttackStat + (current_user.Lvl - 1)* 0.5f;
            current_user.Defence_Point = 5 + item_DefenceStat + (current_user.Lvl - 1);
        }
        // 장비 장착, 레벨업 등으로 인한 스텟 갱신
        public enum MajorType
        {
            도적 = 1,
            전사,
            법사,
            궁수
        }
        // 추후 직업이 추가된다면 직접 추가할 것

        public static string[][] Item_List = new string[6][];
        // 이름 - 설명 - 수치 - 가격 - 소지여부(0미등장 1유저소지 2상점소지) - 장착여부(0미장착 1장착) - 아이템타입(방어구1 무기2)}
        // object(var) 타입으로 선언해서 다양한 타입을 넣는 것도 방법이지만, 매번 타입 선언하는 것보단 낫지 않을까...
        // 앞으론 절대 이런짓하지말고 아래에 던전 데이터처럼 만들자 크아악

        public static void ItemAdd()
        {
            Console.WriteLine("아이템 이름, 설명, 수치, 가격, 소지여부, 장착여부, 아이템타입을 띄어쓰기로 구분해서 넣어 주세요");
            string[] userinput = Console.ReadLine().Split(' ');
            Item_List = Item_List.Append(userinput).ToArray();
        }
        // 아이템을 추가할 때 쓰자. 다만 던전 드랍 같이 유저인풋이 아니라 드랍되게 만드려면 아이템 목록에 미리 넣어놓자,

        public static User current_user;
        // 나중엔 유저 목록도 배열이나 리스트로 만들도록 하자. 저장 기능 구현하면 리스트에서 꺼내올 수 있도록.
        public static List<Dungeon_Data> dungeon_Datas = new List<Dungeon_Data>();
        // 던전 데이터를 구조체(Dungeon_Data)로 선언해 놓고, 리스트로 생성해 보자

        public static void ItemInitializer()
        {
            Item_List[0] = new string[] { "수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", "5", "1000", "2", "0", "1" };
            Item_List[1] = new string[] { "무쇠 갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", "9", "2000", "2", "0", "1" };
            Item_List[2] = new string[] { "스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설이 전해져 내려오는 갑옷입니다.", "15", "5000", "2", "0", "1" };
            Item_List[3] = new string[] { "수련자 검", "수련에 도움을 주는 검입니다.", "2", "4000", "2", "0", "2" };
            Item_List[4] = new string[] { "무쇠 도끼", "무쇠로 만들어져 튼튼한 도끼입니다.", "5", "7000", "2", "0", "2" };
            Item_List[5] = new string[] { "스파르타의 창", "스파르타의 전사들이 사용했다는 전설이 전해져 내려오는 갑옷입니다.", "7", "10000", "2", "0", "2" };
        }
        // 게임 내에 존재하는 아이템 정보를 등록함
        // 이름 - 설명 - 수치 - 가격 - 소지여부(0미등장 1유저소지 2상점소지) - 장착여부(0미장착 1장착) - 아이템타입(방어구1 무기2)}
        // 다음엔 그냥 얌전히 클래스와 리스트 enum를 통해 만들자... 왜 string 배열로 만드는 짓을 했을까

        public static Dungeon_Data Dungeon_Initializer(float failrate, float fail_penelty, int reqired_def, int gold_drop, string name)
        {
            return new Dungeon_Data
            {
                Fail_Rate = failrate,
                Fail_Penelty = fail_penelty,
                Required_Def = reqired_def,
                Gold_Drop = gold_drop,
                Name = name
            };
        }
        // 던전 정보 리스트에 넣기
        static void Main()
        {
            ItemInitializer();
            // 일단 아이템을 불러옴. 다음부턴 아이템도 밑에 던전처럼 하자...
            dungeon_Datas.Add(Dungeon_Initializer(0.3f, 0.3f, 5, 1000, "쉬운 던전"));
            dungeon_Datas.Add(Dungeon_Initializer(0.5f, 0.5f, 11, 1700, "일반 던전"));
            dungeon_Datas.Add(Dungeon_Initializer(0.7f, 0.7f, 17, 2500, "어려운 던전"));
            // 던전 정보 불러옴.

            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            // 던전 입장 시작!
            current_user = MakeCharacter();
            // 캐릭터 생성 시작

            Console.Clear();

            while (true) HomePosition();
            // 메인 홈포지션 시작(무한 반복)
        }
        // 게임 시작 시 초기설정 = 유저 설정, 아이템 설정, 던전 설정. 유니티로 치면 Awake 느낌으로. 실제 게임은 HomePosition() 위주로 진행.
        static public void Shop()
        {
            int userinput;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("============================================");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{current_user.Gold} G\n");

                Console.WriteLine("[아이템 목록]");
                ItemListPrint_Shop(false);
                // 아이템 목록 출력

                Console.WriteLine();

                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                userinput = UserInputHandler(1, true);
                Console.Clear();

                if (userinput == 0) break;
                
                // 상점 구매 창으로 이동
                else if (userinput == 1)
                {
                    while (true)
                    {
                        Console.WriteLine("상점 - 아이템 구매");
                        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                        Console.WriteLine("이미 보유 중 '[x]' 인 아이템을 선택 시 80% 가격에 판매합니다.");
                        Console.WriteLine("============================================");
                        Console.WriteLine("[보유 골드]");
                        Console.WriteLine($"{current_user.Gold} G\n");

                        Console.WriteLine("[아이템 목록]");
                        ItemListPrint_Shop(true);
                        Console.WriteLine();
                        Console.WriteLine("0. 나가기");

                        int itemCount_Shop = 0;
                        for (int i = 0; i < Item_List.Length; i++)
                            if (Item_List[i][4] != "0") itemCount_Shop++;
                        // 미등장한 아이템 제외 모두 카운트

                        userinput = UserInputHandler(itemCount_Shop, true);
                        Console.Clear();
                        if (userinput == 0) break;
                        else
                        {
                            int selected_Item_row = 0;
                            int selected_Item_Price = 0;

                            for (int i = 0; i < Item_List.Length; i++)
                            {
                                if (Item_List[i][4] != "0") selected_Item_row++;
                                // 일단 미등장한 아이템은 넘어가야 함 ([4]=0 제외)
                                if (selected_Item_row == userinput)
                                // 등장한 아이템의 열과 유저 인풋이 일치하면 해당 아이템이 유저꺼(1)인지 / 상점에 있는지(2) 확인
                                {
                                    selected_Item_Price = Int32.Parse(Item_List[i][3]);
                                    // 상점에 있으면, 가격을 체크[3] 해서
                                    if (Item_List[i][4] == "2" && current_user.Gold >= selected_Item_Price)
                                    // 돈 있으면 골드를 차감하고 유저 소지로 변경
                                    {
                                        Console.WriteLine("구매를 완료했습니다.\n");
                                        current_user.Gold -= selected_Item_Price;
                                        Item_List[i][4] = "1";
                                    }
                                    else if (Item_List[i][4] == "2" && current_user.Gold < selected_Item_Price)
                                    // 골드가 없으면 골드가 부족합니다 출력하고 탈출
                                    {
                                        Console.WriteLine("소지 중인 골드가 부족합니다.\n");
                                    }
                                    else if (Item_List[i][4] == "1")
                                    // 유저가 소지 중이면. 가격을 체크[3] 해서
                                    // 가격의 80% 만큼을 얻고
                                    // 소유를 나에서 상점으로 변경 [4] = 1에서 2로 변경
                                    // 그리고 장착 여부를 해제하기 [5] 를 0으로 변경
                                    {
                                        Console.WriteLine("소지 중인 아이템을 판매했습니다.\n");
                                        current_user.Gold += (int)(selected_Item_Price * 0.8);
                                        Item_List[i][4] = "2";
                                        Item_List[i][5] = "0";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        // 상점이랑 인벤토리가 많이 지저분합니다 코드가...
        static public void Inventory()
        {
            int userinput; 
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine("============================================");
                Console.WriteLine("[아이템 목록]");
                ItemListPrint_User(false);
                // 아이템 목록 출력

                Console.WriteLine();

                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                userinput = UserInputHandler(1, true);
                Console.Clear();
                if (userinput == 0) break;
                // HomePosition으로 돌아감
                else if (userinput == 1)
                // 장착 관리 창으로 이동
                while(true)
                {
                    Console.Clear();
                    Console.WriteLine("인벤토리 - 장착 관리");
                    Console.WriteLine("장착을 원하는 아이템을 선택해 주세요.");
                    Console.WriteLine("이미 장착한 아이템을 선택 시 장착을 해제합니다.");
                    Console.WriteLine("============================================");
                    Console.WriteLine("[아이템 목록]");
                    ItemListPrint_User(true);
                    Console.WriteLine();
                    Console.WriteLine("0 : 나가기\n");

                    int selected_Item_row = 0;
                    int selected_Item_type = 0;
                    int changed_Item_row = -1;
                    int itemCount_User = 0;

                    for (int i = 0; i < Item_List.Length; i++)
                        if (Item_List[i][4] == "1") itemCount_User++;
                    // 소지 중인 아이템 수 세기 (나중엔 미리 세 두자. 스파게티;)

                    userinput = UserInputHandler(itemCount_User, true);
                    // 유저 인풋값 받기
                    if (userinput == 0) break;
                    // 0이면 바로 탈출, 아니면 장비 탈착 시작
                    else
                    {
                        for (int i = 0; i < Item_List.Length; i++)
                        {
                            if (Item_List[i][4] == "1")
                                selected_Item_row++;
                            //카운트와 유저인풋 일치하면 해당 열 찾기 완료 = 다음 단계
                            if (selected_Item_row == userinput)
                            {
                                selected_Item_type = Int32.Parse(Item_List[i][6]);
                                if (Item_List[i][5] == "0")
                                {
                                    Item_List[i][5] = "1";
                                    changed_Item_row = i;
                                    break;
                                }
                                else if (Item_List[i][5] == "1")
                                {
                                    Item_List[i][5] = "0";
                                    changed_Item_row = i;
                                    break;
                                }
                                //  중복 체크를 어떻게 할 것인가? : 우선 유저가 체크한 거는 상태 스왑
                            }
                        }

                        for (int i = 0; i < Item_List.Length; i++) // 위에까지는 정상 작동. 다시 순회를 돌면서
                        {
                            if (Int32.Parse(Item_List[i][6]) == selected_Item_type) // 선택한 아이템 목록과 같은 타입의 행에서
                            { 
                                if (Item_List[i][5] == "1" && i != changed_Item_row) // 아이템이 장착 중이면서 + 선택한 항이 아니면 
                                    Item_List[i][5] = "0"; // 장착 해제
                            }
                        }
                    }
                }

            }


        }
        // 상점이랑 인벤토리가 많이 지저분합니다 코드가...

        static public void ItemListPrint_User(bool isEquip) // 장착 창 들어가면 true
            {
                int itemCount = 0;

                for (int i = 0; i < Item_List.Length; i++)
                {
                    if (Item_List[i][4] == "1") // 소지 중인 경우
                    {
                        itemCount++;
                        if (isEquip) Console.Write($" {itemCount}. "); // 장착 창 들어가면 숫자 출력
                        else Console.Write($" - "); // 그 외엔 하이푼

                        if (Item_List[i][5] == "1")
                            Console.Write("[E]");
                        else if (Item_List[i][5] == "0")
                            Console.Write("[ ]");

                        Console.Write($"{Item_List[i][0]} | 방어력 +{Item_List[i][2]} | {Item_List[i][1]}\n");
                    }
                }
                Console.WriteLine();
            }

        static public void ItemListPrint_Shop(bool isBuying) // 구매 창 들어가면 true
            {
                int itemCount = 0;

                for (int i = 0; i < Item_List.Length; i++)
                {
                    if (Item_List[i][4] != "0") // 미등장 아이템 제외 출력
                    {
                        itemCount++;

                        if (isBuying) Console.Write($" {itemCount}. "); // 구매창 들어가면 숫자 출력
                        else Console.Write($" - "); // 그 외엔 하이푼

                        if (Item_List[i][4] == "1") // 유저가 소지 중이면
                            Console.Write("[x]");
                        else if (Item_List[i][4] == "2") // 상점이 소지 중이면
                            Console.Write("[ ]");

                        Console.Write($"{Item_List[i][0]} | 방어력 +{Item_List[i][2]} | 가격 : {Item_List[i][3]}G |{Item_List[i][1]}\n");
                    }
                }
            }

        static public void HomePosition()
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 여러 활동을 하실 수 있습니다.");
                Console.WriteLine("============================================");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전 입장");
                Console.WriteLine("5. 휴식하기");

                int userinput = UserInputHandler(5);

                switch (userinput)
                {
                    case 1:
                        StatApplying();
                        current_user.PrintInfo();
                        break;

                    case 2:
                        Console.Clear();    
                        Inventory();
                        break;

                    case 3:

                        Console.Clear();
                        Shop();
                        break;

                    case 4:
                        Console.Clear();
                        Dungeon_Entry();
                        break;

                    case 5:
                        Console.Clear();
                        Rest();
                        break;

                }

            }
        static public void Dungeon_Entry()
        {
            while (true)
            {
                Console.Clear();
                StatApplying();
                Console.WriteLine("던전 입장");
                Console.WriteLine("충분한 준비를 마치고 던전에 입장하세요.");
                Console.WriteLine("============================================");
                Console.WriteLine($"현재 방어력 : {current_user.Defence_Point} / HP : {current_user.Health_Point}\n");
                
                Console.WriteLine("1. 쉬운 던전 \tㅣ 방어력 5 이상 권장");
                Console.WriteLine("2. 일반 던전 \tㅣ방어력 11 이상 권장");
                Console.WriteLine("3. 어려운 던전\tㅣ방어력 17 이상 권장\n");

                Console.WriteLine("0. 나가기");

                int userinput = UserInputHandler(3, true) - 1; // 던전 리스트에서 편하게 쓰려고 -1

                if (userinput + 1 == 0) break; // 0이면 탈출
                else InDungeonChanges(userinput); // 1,2,3 이면 인던 진행
                
            }
        }

        static public void InDungeonChanges(int dungeon_index)
        {
            Console.Clear();
            Random random = new Random();
            Dungeon_Data dungeon = dungeon_Datas[dungeon_index];

            if (current_user.Defence_Point < dungeon.Required_Def) // 실패 확률 및 실패 시
            {
                if (random.Next(101) < dungeon.Fail_Rate * 100)
                {
                    Console.WriteLine("던전 클리어 : 실패!");
                    Console.WriteLine($"{dungeon.Name} 공략에 실패하셨습니다 :( \n");

                    Console.WriteLine("[탐험 결과]");
                    Console.Write($"체력 : {current_user.Health_Point:N1} -> ");
                    current_user.Health_Point *= (1 - dungeon.Fail_Penelty);
                    Console.WriteLine($"{current_user.Health_Point:N1}\n");

                    Console.WriteLine("0. 나가기");
                    UserInputHandler(0, true);
                }
            }

            else // 성공 시
            {
                Console.WriteLine("던전 클리어");
                Console.WriteLine("축하합니다!");
                Console.WriteLine($"{dungeon.Name} 공략에 성공하셨습니다!\n");
                
                Console.WriteLine("[탐험 결과]");
                Console.Write($"체력 : {current_user.Health_Point:N1} -> ");
                current_user.Health_Point -= (random.Next(20,36) - (current_user.Defence_Point - dungeon.Required_Def));
                Console.WriteLine($"{current_user.Health_Point:N1}");
                
                Console.Write($"골드 : {current_user.Gold} -> ");
                current_user.Gold += (int)(dungeon.Gold_Drop * (1 + current_user.Strength_Point * random.Next(100,201) / 10000));
                Console.WriteLine($"{current_user.Gold}\n");

                current_user.Exp++;
                if (current_user.Exp >= current_user.Lvl)
                {
                    current_user.Lvl++;
                    current_user.Exp = 0;
                    Console.WriteLine("레벨 업!");
                    Console.WriteLine($"현재 레벨 : {current_user.Lvl}\n");
                }
                
                Console.WriteLine("0. 나가기");
                UserInputHandler(0,true);
            }

        }

        static public void Rest()
        {
            while (true)
            {
                Console.WriteLine("휴식하기");
                Console.WriteLine("500 G 를 내면 체력을 회복할 수 있습니다.");
                Console.WriteLine("============================================");
                Console.WriteLine($"보유 골드 : {current_user.Gold} G");
                Console.WriteLine($"현재 HP : {current_user.Health_Point:N1}\n");

                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기\n");

                int userinput = UserInputHandler(1, true);

                if (userinput == 1 && current_user.Gold < 500)
                {
                    Console.WriteLine("골드가 부족합니다.");
                    userinput = UserInputHandler(0, true);
                }

                else if (userinput == 1 && current_user.Gold >= 500)
                {
                    current_user.Gold -= 500;
                    current_user.Health_Point = 100;
                    Console.Clear();
                }

                if (userinput == 0)
                {
                    Console.Clear();
                    break;
                }
            }
        }
        static public User MakeCharacter()
            {
                User current_user = new User();

                Console.WriteLine("캐릭터를 생성합니다.");
                current_user.Name = WriteName();
                Console.Clear();
                current_user.Major = ChooseMajor();
                current_user.Strength_Point = 10;
                current_user.Defence_Point = 5;
                current_user.Health_Point = 100;
                current_user.Gold = 1500;
                current_user.Lvl = 1;

                return current_user;
        }

        static public string WriteName()
            {
                Console.WriteLine("캐릭터의 이름을 입력해 주세요.");
                string newName = Console.ReadLine();


                Console.WriteLine();
                Console.WriteLine($"입력하신 이름은 {newName} 입니다.\n");

                Console.WriteLine("그대로 진행하시겠습니까?");
                Console.WriteLine("1. 확인");
                Console.WriteLine("2. 다시 입력");
                int userinput = UserInputHandler(2);
                if (userinput == 1) return newName;
                else return WriteName();
            }

        static public int ChooseMajor()
            {
                Console.WriteLine($"전직하고 싶은 직업을 선택하세요.\n");

                int howmanyMajors = Enum.GetValues<MajorType>().Length;
                for (int i = 1; i < howmanyMajors + 1; i++)
                {
                    Console.WriteLine($"{i}. {Enum.GetName(typeof(MajorType), i)} ");
                }

                int major_selected = UserInputHandler(howmanyMajors);

                Console.Clear();
                Console.WriteLine($"선택한 직업은 {Enum.GetName(typeof(MajorType), major_selected)} 입니다.");
                Console.WriteLine("그대로 진행하시겠습니까?");
                Console.WriteLine("1. 확인");
                Console.WriteLine("2. 다시 입력");

                int userinput = UserInputHandler(2);

                if (userinput == 1) return major_selected;
                else return ChooseMajor();
            }

        static public int UserInputHandler(int vaild_Input)
            {
            Console.WriteLine("============================================");    
            Console.WriteLine("원하시는 행동을 입력해주세요.");
                string userinput = Console.ReadLine();

                bool isVaildInput = Int32.TryParse(userinput, out int result);

                if (isVaildInput && result > 0 && result <= vaild_Input)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                    return UserInputHandler(vaild_Input);
                }
            }
        // 유저 숫자 인풋 받는 거! 아래는 0(나가기)를 포함하는 경우에 호출하는 오버로드? 라고 하나요?

        static public int UserInputHandler(int vaild_Input, bool isThereZero) // 뒤로 가기 버튼을 위한 0이 있으면 true
        {
            Console.WriteLine("============================================"); 
            Console.WriteLine("원하시는 행동을 입력해주세요.");
                string userinput = Console.ReadLine();

                bool isVaildInput = Int32.TryParse(userinput, out int result);

                if (isVaildInput && result >= 0 && result <= vaild_Input) // 허용값에 0 포함
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                    return UserInputHandler(vaild_Input, isThereZero);
                }
            }
    }

    
}
