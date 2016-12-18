using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{

    [OperationContract]
    [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedResponse, ResponseFormat = WebMessageFormat.Json)]
    void UpdateDBForPlayer(Player player);

    [OperationContract]
    [WebGet(UriTemplate = "StatsFileGenerator/{playerID}", BodyStyle = WebMessageBodyStyle.WrappedResponse, ResponseFormat = WebMessageFormat.Json)]
    //[WebInvoke(Method = "POST", UriTemplate = "StatsFileGenerator/{playerID}", BodyStyle = WebMessageBodyStyle.WrappedResponse, ResponseFormat = WebMessageFormat.Json)]
    String StatsFileGenerator(string playerID);

    [OperationContract]
    [WebGet(UriTemplate = "UpdateDBMethod/{updateDate}/{playerID}", BodyStyle = WebMessageBodyStyle.WrappedResponse, ResponseFormat = WebMessageFormat.Json)]
    int UpdateDBMethod(string updateDate, string playerID);

    [OperationContract]
    [WebGet(UriTemplate = "testService")]
    string testService();

    [OperationContract]
    [WebGet(UriTemplate = "GetPlayerList", BodyStyle = WebMessageBodyStyle.WrappedResponse, ResponseFormat = WebMessageFormat.Json)]
    string GetPlayerList();

    [OperationContract]
    [WebGet(UriTemplate = "GetArticles/{playerID}", BodyStyle = WebMessageBodyStyle.WrappedResponse, ResponseFormat = WebMessageFormat.Json)]
    string GetArticles(string playerID);

}

//Used to store the info for POST style UpdateDBForPlayer Service
[DataContract]
public class Player
{
    //Player ID number
    [DataMember]
    public string playerID { get; set; }

    //Player name
    [DataMember]
    public string name { get; set; }

    //Link to webpage containing player information
    [DataMember]
    public string html { get; set; }

}
