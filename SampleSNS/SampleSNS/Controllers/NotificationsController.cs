using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.Runtime;

namespace SampleSNS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        BasicAWSCredentials credentials = new BasicAWSCredentials("AKIAXLXB3FZEYMSNWBXF", "O41SYhQwXxfM044jybcgQCqZJqGN+gdzUd7ikfAn");

        [HttpGet(Name = "Create SQS")]
        [Route("CreateSQS")]
        public async System.Threading.Tasks.Task<CreateQueueResponse> CreateSQS(string queueName)
        {
            CreateQueueResponse createQueueResponse = new CreateQueueResponse();
            using (AmazonSQSClient sqsClient = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                CreateQueueRequest request = new CreateQueueRequest
                {
                    QueueName = queueName,
                    Attributes = new Dictionary<string, string>
                    {
                        { "VisibilityTimeout","20"}
                    }

                 };
                createQueueResponse = await sqsClient.CreateQueueAsync(request);
            }

            return createQueueResponse;
        }

        [HttpGet(Name = "Get queue attributes")]
        [Route("GetQueueAttribs")]
        public async System.Threading.Tasks.Task<GetQueueAttributesResponse> GetQueueAttributes(string queueUrl)
        {
            GetQueueAttributesResponse queueAttributesResponse = new GetQueueAttributesResponse();
            using (AmazonSQSClient sqsClient = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                GetQueueAttributesRequest request = new GetQueueAttributesRequest
                {
                    QueueUrl = queueUrl,
                    AttributeNames = new List<string>() { "All" }

                };
                queueAttributesResponse = await sqsClient.GetQueueAttributesAsync(request);
            }

            return queueAttributesResponse;
        }

        [HttpGet(Name = "Get list of subscriptions")]
        [Route("GetSubscriptions")]
        public async System.Threading.Tasks.Task<ListSubscriptionsResponse> GetSubscriptionsAsync()
        {
            ListSubscriptionsResponse listSubscription = new ListSubscriptionsResponse();
            using (AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                ListSubscriptionsRequest request = new ListSubscriptionsRequest();

                listSubscription = await snsClient.ListSubscriptionsAsync();
            }

            return listSubscription;
        }

        [HttpGet(Name = "Get list of topics")]
        [Route("GetTopics")]
        public async System.Threading.Tasks.Task<ListTopicsResponse> GetTopicsAsync()
        {
            ListTopicsResponse listTopics = new ListTopicsResponse();
            using (AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                ListTagsForResourceRequest request = new ListTagsForResourceRequest();

                listTopics = await snsClient.ListTopicsAsync();
            }

            return listTopics;
        }

        [HttpGet(Name = "Subscribe SNS to SQS")]
        [Route("SubscribeSNSToSQS")]
        public async System.Threading.Tasks.Task<SubscribeResponse> SubscribeToSQS(string topicARN, string sqsArn, string queueUrl)
        {
            // subcriber SNS to a SQS
            SubscribeResponse subscriptionResponse = new SubscribeResponse();
            using (AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                SubscribeRequest request = new SubscribeRequest()
                {
                    TopicArn = topicARN,
                    Endpoint = sqsArn,
                    Protocol = "sqs"
                };

                subscriptionResponse = await snsClient.SubscribeAsync(request);
            }

            return subscriptionResponse;

            //// allow SQS to recieve message from SNS
            //SetQueueAttributesResponse setAttributeResponse = new SetQueueAttributesResponse();
            //using (AmazonSQSClient sqsClient = new AmazonSQSClient(credentials, Amazon.RegionEndpoint.USEast2))
            //{
            //    Dictionary<string, string> attrs = new Dictionary<string, string>();
            //    attrs.Add(QueueAttributeName.Policy,
            //        "{\"Version\": " + "\"2020-06-27\"," +
            //            "\"Id\":" + "\\" + sqsArn + "/SQSDefaultPolicy\"," +
            //            "\"Statement\":" + " [ {\"Sid\":" + "\"Sid1494310941284\"," +
            //            "\"Effect\":" + "\"Allow\"," +
            //            "\"Principal\":" + "\"*\"," +
            //            "\"Action\":" + "\"SQS:SendMessage\"," +
            //            "\"Resource\":" + "\\" + sqsArn + "\"," +
            //            "\"Condition\":" +
            //            "{\"StringEquals\":" +
            //           "{\"aws:SourceArn\":" + "\\" + topicARN + "\"}} }]}");

            //    SetQueueAttributesRequest setAttributeRequest = new SetQueueAttributesRequest
            //    {
            //        Attributes = attrs,
            //        QueueUrl = queueUrl

            //    };

            //    setAttributeResponse = await sqsClient.SetQueueAttributesAsync(setAttributeRequest);
            //}

            //return setAttributeResponse;
        }

        [HttpGet(Name = "publish a message to a topic")]
        [Route("PublishMessage")]
        public async System.Threading.Tasks.Task<PublishResponse> PublishMessage(string topicArn, string message, string subject)
        {
            PublishResponse publishResponse = new PublishResponse();
            using (AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                PublishRequest request = new PublishRequest()
                {
                    TopicArn = topicArn,
                    Message = "Hi Lorenzo </br></br> Gwapo mo.",
                    Subject = "Sample Subject"
                };

                publishResponse = await snsClient.PublishAsync(request);
            }

            return publishResponse;
        }

    }
}