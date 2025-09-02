using UnityEngine;
           using System.Collections.Generic;
           namespace ES{   
                    
                    public static partial  class ESAssetBundlePath 
                    {
                        /*
 {"material","material_d7d769d0d26dc2d08218221e80553319"},
{"prefab","prefab_8a84a86a5e53eb343dcad48dccd0bffe"},
{"rubbish","rubbish_e59a39171689cacd6c47e973cb49b339"},

*/          
                    public static Dictionary<string, Dictionary<string,string>> AllPathsDic 
=new Dictionary<string, Dictionary<string, string>>()
{{"aaaa",aaaa.AllPaths},
{"material",material.AllPaths},
{"prefab",prefab.AllPaths},
{"rubbish",rubbish.AllPaths},
};

          
                    public static Dictionary<string,string> AllABHashDic 
=new Dictionary<string, string>()
{{"material","material_d7d769d0d26dc2d08218221e80553319"},
{"prefab","prefab_8a84a86a5e53eb343dcad48dccd0bffe"},
{"rubbish","rubbish_e59a39171689cacd6c47e973cb49b339"},
};


                    
                    public static  class aaaa 
                    {
                                  
                    public static Dictionary<string, string>  AllPaths 
=new Dictionary<string, string> {  };


                    }
                

                    
                    public static  class material 
                    {
                                  
                    public static string PreName ="material";

          
                    public static string WithHash ="material_d7d769d0d26dc2d08218221e80553319";

          
                    public static string Hash ="d7d769d0d26dc2d08218221e80553319";

          
                    public static string Blue_mat ="Blue";

          
                    public static string Yellow_mat ="Yellow";

          
                    public static Dictionary<string, string>  AllPaths 
=new Dictionary<string, string> { {"Blue_mat" ,"Blue"},

{"Yellow_mat" ,"Yellow"},
 };


                    }
                

                    
                    public static  class prefab 
                    {
                                  
                    public static string PreName ="prefab";

          
                    public static string WithHash ="prefab_8a84a86a5e53eb343dcad48dccd0bffe";

          
                    public static string Hash ="8a84a86a5e53eb343dcad48dccd0bffe";

          
                    public static string 方块_1_mat ="方块 1";

          
                    public static string 方块_2_prefab ="方块 2";

          
                    public static string 方块_3_prefab ="方块 3";

          
                    public static string 方块_4_prefab ="方块 4";

          
                    public static string 方块_5_prefab ="方块 5";

          
                    public static string 方块_prefab ="方块";

          
                    public static string 方块_1_prefab ="方块 1";

          
                    public static string 方块_2_prefab_1 ="方块 2";

          
                    public static string 方块_3_prefab_1 ="方块 3";

          
                    public static string 方块_4_prefab_1 ="方块 4";

          
                    public static string 方块_5_prefab_1 ="方块 5";

          
                    public static string 方块_prefab_1 ="方块";

          
                    public static string _核心_不死系统核心_prefab ="【核心】不死系统核心";

          
                    public static string _核心_网络支持_非联网_prefab ="【核心】网络支持(非联网)";

          
                    public static string 网络功能支持_联网_prefab ="网络功能支持(联网)";

          
                    public static string 一组_prefab ="一组";

          
                    public static string 一组10_4_25_prefab ="一组10_4_25_";

          
                    public static string 实体测试_prefab ="实体测试";

          
                    public static Dictionary<string, string>  AllPaths 
=new Dictionary<string, string> { {"方块_1_mat" ,"方块 1"},

{"方块_2_prefab" ,"方块 2"},
{"方块_3_prefab" ,"方块 3"},
{"方块_4_prefab" ,"方块 4"},
{"方块_5_prefab" ,"方块 5"},

{"方块_prefab" ,"方块"},
{"方块_1_prefab" ,"方块 1"},
{"方块_2_prefab_1" ,"方块 2"},
{"方块_3_prefab_1" ,"方块 3"},

{"方块_4_prefab_1" ,"方块 4"},
{"方块_5_prefab_1" ,"方块 5"},
{"方块_prefab_1" ,"方块"},
{"_核心_不死系统核心_prefab" ,"【核心】不死系统核心"},

{"_核心_网络支持_非联网_prefab" ,"【核心】网络支持(非联网)"},
{"网络功能支持_联网_prefab" ,"网络功能支持(联网)"},
{"一组_prefab" ,"一组"},
{"一组10_4_25_prefab" ,"一组10_4_25_"},

{"实体测试_prefab" ,"实体测试"},
 };


                    }
                

                    
                    public static  class rubbish 
                    {
                                  
                    public static string PreName ="rubbish";

          
                    public static string WithHash ="rubbish_e59a39171689cacd6c47e973cb49b339";

          
                    public static string Hash ="e59a39171689cacd6c47e973cb49b339";

          
                    public static string buildingDataGroup_cs ="buildingDataGroup";

          
                    public static string TestOnlyDataGroup_cs ="TestOnlyDataGroup";

          
                    public static string buildingDataInfo_cs ="buildingDataInfo";

          
                    public static string TestOnlyDataInfo_cs ="TestOnlyDataInfo";

          
                    public static string buildingDataPack_cs ="buildingDataPack";

          
                    public static string TestOnlyDataPack_cs ="TestOnlyDataPack";

          
                    public static string Buff新建数据包51407_asset ="Buff新建数据包51407";

          
                    public static string 新建建筑数据组19359_asset ="新建建筑数据组19359";

          
                    public static Dictionary<string, string>  AllPaths 
=new Dictionary<string, string> { {"buildingDataGroup_cs" ,"buildingDataGroup"},

{"TestOnlyDataGroup_cs" ,"TestOnlyDataGroup"},
{"buildingDataInfo_cs" ,"buildingDataInfo"},
{"TestOnlyDataInfo_cs" ,"TestOnlyDataInfo"},
{"buildingDataPack_cs" ,"buildingDataPack"},

{"TestOnlyDataPack_cs" ,"TestOnlyDataPack"},
{"Buff新建数据包51407_asset" ,"Buff新建数据包51407"},
{"新建建筑数据组19359_asset" ,"新建建筑数据组19359"},
 };


                    }
                

                    }
                }
