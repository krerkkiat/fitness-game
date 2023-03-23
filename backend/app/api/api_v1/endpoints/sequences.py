from typing import Any

from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session

from app import crud, models, schemas
from app.api import deps

router = APIRouter()


@router.get("/", response_model=list[schemas.Sequence])
def read_sequences(
    db: Session = Depends(deps.get_db), skip: int = 0, limit: int = 100
) -> Any:
    return crud.sequence.get_multi(db, skip=skip, limit=limit)


@router.post("/", response_model=schemas.Sequence)
def create_sequence(
    *, db: Session = Depends(deps.get_db), sequence_in: schemas.SequenceCreate
) -> Any:
    sequence = crud.sequence.create(db=db, obj_in=sequence_in)
    return sequence
