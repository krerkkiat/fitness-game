from google.protobuf.internal import containers as _containers
from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from typing import ClassVar as _ClassVar, Iterable as _Iterable, Mapping as _Mapping, Optional as _Optional, Union as _Union

DESCRIPTOR: _descriptor.FileDescriptor

class Quat(_message.Message):
    __slots__ = ["w", "x", "y", "z"]
    W_FIELD_NUMBER: _ClassVar[int]
    X_FIELD_NUMBER: _ClassVar[int]
    Y_FIELD_NUMBER: _ClassVar[int]
    Z_FIELD_NUMBER: _ClassVar[int]
    w: float
    x: float
    y: float
    z: float
    def __init__(self, w: _Optional[float] = ..., x: _Optional[float] = ..., y: _Optional[float] = ..., z: _Optional[float] = ...) -> None: ...

class Pose(_message.Message):
    __slots__ = ["x", "y", "z", "quat"]
    X_FIELD_NUMBER: _ClassVar[int]
    Y_FIELD_NUMBER: _ClassVar[int]
    Z_FIELD_NUMBER: _ClassVar[int]
    QUAT_FIELD_NUMBER: _ClassVar[int]
    x: float
    y: float
    z: float
    quat: Quat
    def __init__(self, x: _Optional[float] = ..., y: _Optional[float] = ..., z: _Optional[float] = ..., quat: _Optional[_Union[Quat, _Mapping]] = ...) -> None: ...

class Cube(_message.Message):
    __slots__ = ["id", "pose"]
    ID_FIELD_NUMBER: _ClassVar[int]
    POSE_FIELD_NUMBER: _ClassVar[int]
    id: int
    pose: Pose
    def __init__(self, id: _Optional[int] = ..., pose: _Optional[_Union[Pose, _Mapping]] = ...) -> None: ...

class Tap(_message.Message):
    __slots__ = ["cube_id", "timestamp"]
    CUBE_ID_FIELD_NUMBER: _ClassVar[int]
    TIMESTAMP_FIELD_NUMBER: _ClassVar[int]
    cube_id: int
    timestamp: int
    def __init__(self, cube_id: _Optional[int] = ..., timestamp: _Optional[int] = ...) -> None: ...

class Sequence(_message.Message):
    __slots__ = ["name", "cubes", "taps"]
    NAME_FIELD_NUMBER: _ClassVar[int]
    CUBES_FIELD_NUMBER: _ClassVar[int]
    TAPS_FIELD_NUMBER: _ClassVar[int]
    name: str
    cubes: _containers.RepeatedCompositeFieldContainer[Cube]
    taps: _containers.RepeatedCompositeFieldContainer[Tap]
    def __init__(self, name: _Optional[str] = ..., cubes: _Optional[_Iterable[_Union[Cube, _Mapping]]] = ..., taps: _Optional[_Iterable[_Union[Tap, _Mapping]]] = ...) -> None: ...

class AddSequenceRequest(_message.Message):
    __slots__ = ["username", "sequence"]
    USERNAME_FIELD_NUMBER: _ClassVar[int]
    SEQUENCE_FIELD_NUMBER: _ClassVar[int]
    username: str
    sequence: Sequence
    def __init__(self, username: _Optional[str] = ..., sequence: _Optional[_Union[Sequence, _Mapping]] = ...) -> None: ...

class AddSequenceReply(_message.Message):
    __slots__ = ["message"]
    MESSAGE_FIELD_NUMBER: _ClassVar[int]
    message: str
    def __init__(self, message: _Optional[str] = ...) -> None: ...

class GetSequenceListRequest(_message.Message):
    __slots__ = ["begin", "count"]
    BEGIN_FIELD_NUMBER: _ClassVar[int]
    COUNT_FIELD_NUMBER: _ClassVar[int]
    begin: int
    count: int
    def __init__(self, begin: _Optional[int] = ..., count: _Optional[int] = ...) -> None: ...

class GetSequenceListReply(_message.Message):
    __slots__ = ["sequences"]
    SEQUENCES_FIELD_NUMBER: _ClassVar[int]
    sequences: _containers.RepeatedCompositeFieldContainer[Sequence]
    def __init__(self, sequences: _Optional[_Iterable[_Union[Sequence, _Mapping]]] = ...) -> None: ...
