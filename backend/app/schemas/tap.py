from typing import Optional

from pydantic import BaseModel

# Shared properties
class TapBase(BaseModel):
    cube_id: int
    timestamp: int

# Properties to receive on item creation
class TapCreate(TapBase):
    pass

# Properties to receive on item update
class TapUpdate(TapBase):
    pass


# Properties shared by models stored in DB
class TapInDBBase(TapBase):
    id: int

    class Config:
        orm_mode = True


# Properties to return to client
class Tap(TapInDBBase):
    pass


# Properties stored in DB
class TapInDB(TapInDBBase):
    pass