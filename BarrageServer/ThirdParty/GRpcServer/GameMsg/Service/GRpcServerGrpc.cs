// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: GRpcServer.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace GameMain.Msg {
  public static partial class GRpcServer
  {
    static readonly string __ServiceName = "GameMain.Msg.GRpcServer";

    static readonly grpc::Marshaller<global::GameMain.Msg.GRpcMsg> __Marshaller_GameMain_Msg_GRpcMsg = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::GameMain.Msg.GRpcMsg.Parser.ParseFrom);

    static readonly grpc::Method<global::GameMain.Msg.GRpcMsg, global::GameMain.Msg.GRpcMsg> __Method_Handle = new grpc::Method<global::GameMain.Msg.GRpcMsg, global::GameMain.Msg.GRpcMsg>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Handle",
        __Marshaller_GameMain_Msg_GRpcMsg,
        __Marshaller_GameMain_Msg_GRpcMsg);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::GameMain.Msg.GRpcServerReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of GRpcServer</summary>
    [grpc::BindServiceMethod(typeof(GRpcServer), "BindService")]
    public abstract partial class GRpcServerBase
    {
      public virtual global::System.Threading.Tasks.Task<global::GameMain.Msg.GRpcMsg> Handle(global::GameMain.Msg.GRpcMsg request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for GRpcServer</summary>
    public partial class GRpcServerClient : grpc::ClientBase<GRpcServerClient>
    {
      /// <summary>Creates a new client for GRpcServer</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public GRpcServerClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for GRpcServer that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public GRpcServerClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected GRpcServerClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected GRpcServerClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::GameMain.Msg.GRpcMsg Handle(global::GameMain.Msg.GRpcMsg request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Handle(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::GameMain.Msg.GRpcMsg Handle(global::GameMain.Msg.GRpcMsg request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Handle, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::GameMain.Msg.GRpcMsg> HandleAsync(global::GameMain.Msg.GRpcMsg request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return HandleAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::GameMain.Msg.GRpcMsg> HandleAsync(global::GameMain.Msg.GRpcMsg request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Handle, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override GRpcServerClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new GRpcServerClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(GRpcServerBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Handle, serviceImpl.Handle).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, GRpcServerBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Handle, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GameMain.Msg.GRpcMsg, global::GameMain.Msg.GRpcMsg>(serviceImpl.Handle));
    }

  }
}
#endregion
