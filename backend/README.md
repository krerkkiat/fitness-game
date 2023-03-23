# Backend of fitness-game

## Setting up development environemnt

```console
python -m pip install -r requirements.txt
PYTHONPATH=app uvicorn main:app --reload
```

For PowerShell, need `$env:PYTHONPATH="app"`.

## Templates Used

- https://github.com/tiangolo/full-stack-fastapi-postgresql/tree/master/%7B%7Bcookiecutter.project_slug%7D%7D/backend
