from typing import TYPE_CHECKING

from sqlalchemy import Column, ForeignKey, Integer, String
from sqlalchemy.orm import relationship, Mapped, mapped_column

from app.db.base_class import Base

if TYPE_CHECKING:
    from .sequence import Sequence  # noqa: F401


class Tap(Base):
    id: Mapped[int] = mapped_column(primary_key=True, index=True)
    cube_id: Mapped[int] = mapped_column()
    timestamp: Mapped[int] = mapped_column()
    sequence_id: Mapped[int] = mapped_column(ForeignKey("sequence.id"))
    sequence: Mapped["Sequence"] = relationship(back_populates="taps")
