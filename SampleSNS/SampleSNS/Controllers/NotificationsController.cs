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
        BasicAWSCredentials credentials = new BasicAWSCredentials("sample", "sample");

        [HttpGet(Name = "Create Topic")]
        [Route("CreateTopic")]
        public async System.Threading.Tasks.Task<CreateTopicResponse> CreateTopic(string topicName)
        {
            CreateTopicResponse createTopicResponse = new CreateTopicResponse();
            using (AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                CreateTopicRequest createTopicRequest = new CreateTopicRequest(topicName);
                createTopicResponse = await snsClient.CreateTopicAsync(createTopicRequest);
            }

            return createTopicResponse;
        }

        [HttpGet(Name = "Subscribe to a topic")]
        [Route("SubscribeToTopic")]
        public async System.Threading.Tasks.Task<SubscribeResponse> SubscribeToTopic(string topicArn, string emailAddress)
        {
            SubscribeResponse subscribeResponse = new SubscribeResponse();
            using (AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                SubscribeRequest subscribeRequest = new SubscribeRequest(topicArn, "email", emailAddress);
                subscribeResponse = await snsClient.SubscribeAsync(subscribeRequest);
            }

            return subscribeResponse;
        }


        [HttpGet(Name = "Delete a topic")]
        [Route("DeleteTopic")]
        public async System.Threading.Tasks.Task<DeleteTopicResponse> DeleteTopic(string topicArn)
        {
            DeleteTopicResponse deleteTopicResponse = new DeleteTopicResponse();
            using (AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                DeleteTopicRequest deleteRequest = new DeleteTopicRequest(topicArn);
                deleteTopicResponse = await snsClient.DeleteTopicAsync(deleteRequest);
            }

            return deleteTopicResponse;
        }

        [HttpGet(Name = "Unsubscribe to a topic")]
        [Route("Unsubscribe")]
        public async System.Threading.Tasks.Task<UnsubscribeResponse> Unsubcribe(string subscriptionArn)
        {
            UnsubscribeResponse unsubscribeResponse = new UnsubscribeResponse();
            using (AmazonSimpleNotificationServiceClient snsClient = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.USEast2))
            {
                UnsubscribeRequest unsubscribeRequest = new UnsubscribeRequest(subscriptionArn);
                unsubscribeResponse = await snsClient.UnsubscribeAsync(unsubscribeRequest);
            }

            return unsubscribeResponse;
        }

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
