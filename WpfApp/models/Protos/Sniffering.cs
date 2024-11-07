// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/sniffering.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace GrpcClient {

  /// <summary>Holder for reflection information generated from Protos/sniffering.proto</summary>
  public static partial class SnifferingReflection {

    #region Descriptor
    /// <summary>File descriptor for Protos/sniffering.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static SnifferingReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdQcm90b3Mvc25pZmZlcmluZy5wcm90bxIHc25pZmZlchobZ29vZ2xlL3By",
            "b3RvYnVmL2VtcHR5LnByb3RvIh8KDnN0cmVhbWluZ1JlcGx5Eg0KBWluZGV4",
            "GAEgASgFIogBChBzdHJlYW1pbmdSZXF1ZXN0EhQKC3NvdXJjZV9wb3J0GJA/",
            "IAEoBRISCglkZXN0X3BvcnQYkT8gASgFEhIKCXNvdXJjZV9pcBjAASABKAkS",
            "EAoHZGVzdF9pcBioASABKAkSEgoKc291cmNlX21hYxgBIAEoCRIQCghkZXN0",
            "X21hYxgCIAEoCTJaCg5TdHJlYW1pbmdEYXRlcxJIChBHZXRTdHJlYW1pbmdE",
            "YXRhEhkuc25pZmZlci5zdHJlYW1pbmdSZXF1ZXN0Ghcuc25pZmZlci5zdHJl",
            "YW1pbmdSZXBseSgBQg2qAgpHcnBjQ2xpZW50YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.EmptyReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::GrpcClient.streamingReply), global::GrpcClient.streamingReply.Parser, new[]{ "Index" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::GrpcClient.streamingRequest), global::GrpcClient.streamingRequest.Parser, new[]{ "SourcePort", "DestPort", "SourceIp", "DestIp", "SourceMac", "DestMac" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class streamingReply : pb::IMessage<streamingReply>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<streamingReply> _parser = new pb::MessageParser<streamingReply>(() => new streamingReply());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<streamingReply> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GrpcClient.SnifferingReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public streamingReply() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public streamingReply(streamingReply other) : this() {
      index_ = other.index_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public streamingReply Clone() {
      return new streamingReply(this);
    }

    /// <summary>Field number for the "index" field.</summary>
    public const int IndexFieldNumber = 1;
    private int index_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Index {
      get { return index_; }
      set {
        index_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as streamingReply);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(streamingReply other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Index != other.Index) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Index != 0) hash ^= Index.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Index != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Index);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Index != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Index);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Index != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Index);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(streamingReply other) {
      if (other == null) {
        return;
      }
      if (other.Index != 0) {
        Index = other.Index;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Index = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Index = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class streamingRequest : pb::IMessage<streamingRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<streamingRequest> _parser = new pb::MessageParser<streamingRequest>(() => new streamingRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<streamingRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::GrpcClient.SnifferingReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public streamingRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public streamingRequest(streamingRequest other) : this() {
      sourcePort_ = other.sourcePort_;
      destPort_ = other.destPort_;
      sourceIp_ = other.sourceIp_;
      destIp_ = other.destIp_;
      sourceMac_ = other.sourceMac_;
      destMac_ = other.destMac_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public streamingRequest Clone() {
      return new streamingRequest(this);
    }

    /// <summary>Field number for the "source_port" field.</summary>
    public const int SourcePortFieldNumber = 8080;
    private int sourcePort_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int SourcePort {
      get { return sourcePort_; }
      set {
        sourcePort_ = value;
      }
    }

    /// <summary>Field number for the "dest_port" field.</summary>
    public const int DestPortFieldNumber = 8081;
    private int destPort_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int DestPort {
      get { return destPort_; }
      set {
        destPort_ = value;
      }
    }

    /// <summary>Field number for the "source_ip" field.</summary>
    public const int SourceIpFieldNumber = 192;
    private string sourceIp_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string SourceIp {
      get { return sourceIp_; }
      set {
        sourceIp_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "dest_ip" field.</summary>
    public const int DestIpFieldNumber = 168;
    private string destIp_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string DestIp {
      get { return destIp_; }
      set {
        destIp_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "source_mac" field.</summary>
    public const int SourceMacFieldNumber = 1;
    private string sourceMac_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string SourceMac {
      get { return sourceMac_; }
      set {
        sourceMac_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "dest_mac" field.</summary>
    public const int DestMacFieldNumber = 2;
    private string destMac_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string DestMac {
      get { return destMac_; }
      set {
        destMac_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as streamingRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(streamingRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (SourcePort != other.SourcePort) return false;
      if (DestPort != other.DestPort) return false;
      if (SourceIp != other.SourceIp) return false;
      if (DestIp != other.DestIp) return false;
      if (SourceMac != other.SourceMac) return false;
      if (DestMac != other.DestMac) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (SourcePort != 0) hash ^= SourcePort.GetHashCode();
      if (DestPort != 0) hash ^= DestPort.GetHashCode();
      if (SourceIp.Length != 0) hash ^= SourceIp.GetHashCode();
      if (DestIp.Length != 0) hash ^= DestIp.GetHashCode();
      if (SourceMac.Length != 0) hash ^= SourceMac.GetHashCode();
      if (DestMac.Length != 0) hash ^= DestMac.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (SourceMac.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(SourceMac);
      }
      if (DestMac.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(DestMac);
      }
      if (DestIp.Length != 0) {
        output.WriteRawTag(194, 10);
        output.WriteString(DestIp);
      }
      if (SourceIp.Length != 0) {
        output.WriteRawTag(130, 12);
        output.WriteString(SourceIp);
      }
      if (SourcePort != 0) {
        output.WriteRawTag(128, 249, 3);
        output.WriteInt32(SourcePort);
      }
      if (DestPort != 0) {
        output.WriteRawTag(136, 249, 3);
        output.WriteInt32(DestPort);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (SourceMac.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(SourceMac);
      }
      if (DestMac.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(DestMac);
      }
      if (DestIp.Length != 0) {
        output.WriteRawTag(194, 10);
        output.WriteString(DestIp);
      }
      if (SourceIp.Length != 0) {
        output.WriteRawTag(130, 12);
        output.WriteString(SourceIp);
      }
      if (SourcePort != 0) {
        output.WriteRawTag(128, 249, 3);
        output.WriteInt32(SourcePort);
      }
      if (DestPort != 0) {
        output.WriteRawTag(136, 249, 3);
        output.WriteInt32(DestPort);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (SourcePort != 0) {
        size += 3 + pb::CodedOutputStream.ComputeInt32Size(SourcePort);
      }
      if (DestPort != 0) {
        size += 3 + pb::CodedOutputStream.ComputeInt32Size(DestPort);
      }
      if (SourceIp.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeStringSize(SourceIp);
      }
      if (DestIp.Length != 0) {
        size += 2 + pb::CodedOutputStream.ComputeStringSize(DestIp);
      }
      if (SourceMac.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(SourceMac);
      }
      if (DestMac.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DestMac);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(streamingRequest other) {
      if (other == null) {
        return;
      }
      if (other.SourcePort != 0) {
        SourcePort = other.SourcePort;
      }
      if (other.DestPort != 0) {
        DestPort = other.DestPort;
      }
      if (other.SourceIp.Length != 0) {
        SourceIp = other.SourceIp;
      }
      if (other.DestIp.Length != 0) {
        DestIp = other.DestIp;
      }
      if (other.SourceMac.Length != 0) {
        SourceMac = other.SourceMac;
      }
      if (other.DestMac.Length != 0) {
        DestMac = other.DestMac;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            SourceMac = input.ReadString();
            break;
          }
          case 18: {
            DestMac = input.ReadString();
            break;
          }
          case 1346: {
            DestIp = input.ReadString();
            break;
          }
          case 1538: {
            SourceIp = input.ReadString();
            break;
          }
          case 64640: {
            SourcePort = input.ReadInt32();
            break;
          }
          case 64648: {
            DestPort = input.ReadInt32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            SourceMac = input.ReadString();
            break;
          }
          case 18: {
            DestMac = input.ReadString();
            break;
          }
          case 1346: {
            DestIp = input.ReadString();
            break;
          }
          case 1538: {
            SourceIp = input.ReadString();
            break;
          }
          case 64640: {
            SourcePort = input.ReadInt32();
            break;
          }
          case 64648: {
            DestPort = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
