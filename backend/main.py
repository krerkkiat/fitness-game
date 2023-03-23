from fastapi import FastAPI

app = FastAPI()

@app.get("/")
async def root():
    return {"message": "Hello World"}

@app.get("/sequences/")
async def list_sequences():
    return []


@app.get("/sequences/{sequence_id}")
async def get_sequence(sequence_id: int):
    return {""}