from typing import Any

from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session

from app import schemas

router = APIRouter()


@router.get("/", response_model=list[schemas.Sequence])
def read_sequences() -> Any:
    return []
