from typing import Any

from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session

from app import schemas

router = APIRouter()


@router.get("/")
def read_cat() -> Any:
    return {"cat": "meow!"}
