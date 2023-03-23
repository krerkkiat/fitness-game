from typing import TYPE_CHECKING

from sqlalchemy import Column, ForeignKey, Integer, String
from sqlalchemy.orm import relationship, Mapped, mapped_column

from app.db.base_class import Base

if TYPE_CHECKING:
    from .cube import Cube  # noqa: F401
    from .tap import Tap  # noqa: F401


class Sequence(Base):
    id: Mapped[int] = mapped_column(primary_key=True, index=True)
    username: Mapped[str] = mapped_column(index=True)
    cubes: Mapped[list["Cube"]] = relationship(back_populates="sequence")
    taps: Mapped[list["Tap"]] = relationship(back_populates="sequence")
