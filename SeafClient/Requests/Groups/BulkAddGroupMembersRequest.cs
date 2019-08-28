using SeafClient.Requests.UserAccountInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SeafClient.Requests.Groups
{
    /// <summary>
    /// Request to add multiple users to a group (requires API v.2.1)
    /// </summary>
    /// <returns>A container class with Failed and Success arrays.</returns>
    public class BulkAddGroupMemberRequest : SessionRequest<BulkAddGroupMemberResponse>
    {
        public override string CommandUri => String.Format("api/v2.1/groups/{0:d}/members/bulk/", GroupId);

        public override HttpAccessMethod HttpAccessMethod => HttpAccessMethod.Post;

        public int GroupId;

        public IEnumerable<string> UserNames;
        public string Emails;

        public BulkAddGroupMemberRequest(string authToken, int groupId, IEnumerable<string> userNames) : base(authToken)
        {
            GroupId = groupId;
            UserNames = userNames;
            Emails = string.Join(",", userNames);
        }

        public override IEnumerable<KeyValuePair<string, string>> GetBodyParameters()
        {
            yield return new KeyValuePair<string, string>("emails", Emails);
        }

    }

    /// <summary>
    /// A container class with Failed and Success arrays.
    /// </summary>
    public class BulkAddGroupMemberResponse
    {
        public FailedAddGroupMember[] Failed { get; set; }
        public SuccessAddGroupMember[] Success { get; set; }
    }

    /// <summary>
    /// A record for failed attempt to add group member. 
    /// </summary>
    public class FailedAddGroupMember
    {
        public string error_msg { get; set; }
        public string email { get; set; }
    }

    /// <summary>
    /// Details of a successfully added group member, almost equivalent to <c>AccountInfo</c>
    /// </summary>
    public class SuccessAddGroupMember
    {
        public string login_id { get; set; }
        public string name { get; set; }
        public string avatar_url { get; set; }
        public bool is_admin { get; set; }
        public string contact_email { get; set; }
        public string email { get; set; }
    }

}
