from typing import List
import json

from fastapi.encoders import jsonable_encoder
from sqlalchemy.orm import Session

from app.crud.base import CRUDBase
from app.models.sequence import Sequence
from app.models.cube import Cube
from app.models.tap import Tap
from app.schemas.sequence import SequenceCreate, SequenceUpdate


class CRUDSequence(CRUDBase[Sequence, SequenceCreate, SequenceUpdate]):
    def create(self, db: Session, *, obj_in: SequenceCreate) -> Sequence:
        obj_in_data = jsonable_encoder(obj_in)

        # Create cubes.
        cubes = []
        for cube in obj_in_data["cubes"]:
            # Convert pose to string.
            pose_str = json.dumps(cube["pose"])
            cube["pose"] = pose_str

            c = Cube(**cube)
            cubes.append(c)

        # Create taps.
        taps = []
        for tap in obj_in_data["taps"]:
            t = Tap(**tap)
            taps.append(t)

        obj_in_data["cubes"] = cubes
        obj_in_data["taps"] = taps

        db_obj = self.model(**obj_in_data)  # type: ignore
        db.add(db_obj)
        db.commit()

        db.refresh(db_obj)
        for c in cubes:
            db.refresh(c)
        for t in taps:
            db.refresh(t)

        return db_obj


sequence = CRUDSequence(Sequence)
