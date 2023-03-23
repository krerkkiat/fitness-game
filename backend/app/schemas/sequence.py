from typing import Optional

from pydantic import BaseModel

from .tap import TapBase, Tap
from .cube import CubeBase, Cube


# Shared properties
class SequenceBase(BaseModel):
    username: str


# Properties to receive on item creation
class SequenceCreate(SequenceBase):
    cubes: list[CubeBase]
    taps: list[TapBase]


# Properties to receive on item update
class SequenceUpdate(SequenceBase):
    cubes: list[CubeBase]
    taps: list[TapBase]


# Properties shared by models stored in DB
class SequenceInDBBase(SequenceBase):
    id: int
    cubes: list[Cube]
    taps: list[Tap]

    class Config:
        orm_mode = True
        read_with_orm_mode = True


# Properties to return to client
class Sequence(SequenceInDBBase):
    pass


# Additional properties stored in DB
class SequenceInDB(SequenceInDBBase):
    pass
