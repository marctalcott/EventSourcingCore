// using System;
// using Ezley.Events;
// using Ezley.EventSourcing;
// using Ezley.ProjectionStore;
//
// namespace Ezley.Projections
// {
//     public class Auth0UserView
//     {
//         public string Id { get; set; }
//         public Guid UserId { get; set; }
//          
//     }
//
//     public class Auth0UserProjection : Projection<Auth0UserView>
//     {
//         public Auth0UserProjection()
//         {
//             RegisterHandler<Auth0UserRegistered>(WhenAuth0UserRegistered);
//             RegisterHandler<Auth0UserUserIdChanged>(WhenUserAuth0UserUserIdChanged);
//             RegisterHandler<UserAuth0IdChanged>(WhenUserAuth0IdChanged);
//         }
//         public override string[] GetViewNames(string streamId, IEvent @event)
//         {
//             string auth0Id = GetAuth0Id((dynamic)@event);
//             string eventName = nameof(Auth0UserView);
//             var names = new string[] {$"{eventName}:{auth0Id}"};
//             return names;
//         }
//         
//         private string GetAuth0Id(Auth0UserRegistered @event)
//         {
//             return @event.Id;
//         }
//
//         private string GetAuth0Id(Auth0UserUserIdChanged @event)
//         {
//             return @event.Id;
//         }
//         
//         private string GetAuth0Id(UserAuth0IdChanged @event)
//         {
//             return @event.Auth0Id;
//         }
//         
//         
//         private void WhenAuth0UserRegistered(Auth0UserRegistered e, Auth0UserView view)
//         {
//             view.Id = e.Id;
//             view.UserId = e.UserId;
//         }
//         private void WhenUserAuth0UserUserIdChanged(Auth0UserUserIdChanged e, Auth0UserView view)
//         {
//             view.UserId = e.UserId;
//         }
//
//         private void WhenUserAuth0IdChanged(UserAuth0IdChanged e, Auth0UserView view)
//         {
//             view.Id = e.Auth0Id;
//         }
//     }    
// }