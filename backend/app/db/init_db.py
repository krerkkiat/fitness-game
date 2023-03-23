from sqlalchemy.orm import Session

from app import schemas
from app.core.config import settings
from app.db.base import Base
from app.db.session import engine


def init_db(db: Session) -> None:
    # Not using alembic yet.
    Base.metadata.create_all(bind=engine)
