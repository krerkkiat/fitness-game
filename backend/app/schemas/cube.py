from typing import Tuple

from pydantic import BaseModel

# Shared properties
class CubeBase(BaseModel):
    cube_id: int
    pose: Tuple[float, float, float, float, float, float]


# Properties to receive on item creation
class CubeCreate(CubeBase):
    pass

# Properties to receive on item update
class CubeUpdate(CubeBase):
    pass


# Properties shared by models stored in DB
class CubeInDBBase(CubeBase):
    id: int

    class Config:
        orm_mode = True


# Properties to return to client
class Cube(CubeInDBBase):
    pass


# Properties stored in DB
class TapInDB(TapInDBBase):
    pass