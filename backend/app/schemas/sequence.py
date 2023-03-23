from typing import Optional

from pydantic import BaseModel

from .tap import TapBase
from .cube import CubeBase

# Shared properties
class SequenceBase(BaseModel):
    username: str
    cubes: list[CubeBase]
    taps: list[TapBase]

# Properties to receive on item creation
class SequenceCreate(SequenceBase):
    pass

# Properties to receive on item update
class SequenceUpdate(SequenceBase):
    pass


# Properties shared by models stored in DB
class SequenceInDBBase(SequenceBase):
    id: int

    class Config:
        orm_mode = True


# Properties to return to client
class Sequence(SequenceInDBBase):
    pass
