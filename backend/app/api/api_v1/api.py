from fastapi import APIRouter

from app.api.api_v1.endpoints import sequences, cat

api_router = APIRouter()
api_router.include_router(cat.router, prefix="/cat", tags=["cat"])
api_router.include_router(sequences.router, prefix="/sequences", tags=["sequences"])
