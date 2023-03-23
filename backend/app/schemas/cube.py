from typing import Tuple

from pydantic import BaseModel


# Shared properties
class CubeBase(BaseModel):
    cube_id: int


# Properties to receive on item creation
class CubeCreate(CubeBase):
    pose: Tuple[float, float, float, float, float, float]


# Properties to receive on item update
class CubeUpdate(CubeBase):
    pose: Tuple[float, float, float, float, float, float]


# Properties shared by models stored in DB
class CubeInDBBase(CubeBase):
    id: int
    pose: str

    class Config:
        orm_mode = True


# Properties to return to client
class Cube(CubeInDBBase):
    pass


# Additional properties stored in DB
class TapInDB(CubeInDBBase):
    pass
