# Introduction

Conductor is a workflow server built upon [Workflow Core](https://github.com/danielgerlag/workflow-core) that enables you to coordinate multiple services and scripts into workflows so that you can rapidly create complex workflow applications. Workflows are made up of a series of steps, with an internal data object shared between them to pass information around. Conductor automatically runs and tracks each step, and retries when there are errors.

Workflows are written in either JSON or YAML and then added to Conductor's internal registry via the definition API. Then you use the workflow API to invoke them with or without custom data.

# Installation

Conductor is available as a Docker image - `danielgerlag/conductor`

Conductor uses MongoDB as it's datastore, you will also need an instance of MongoDB in order to run Conductor.

Use this command to start a container (with the API available on port 5001) that points to `mongodb://my-mongo-server:27017/` as it's datastore.

```
$ docker run -p 127.0.0.1:5001:80/tcp --env dbhost=mongodb://my-mongo-server:27017/ danielgerlag/conductor
```

If you wish to run a fleet of Conductor nodes, then you also need to have a Redis instance, which they will use as a backplane. This is not required if you are only running one instance.
Simply have all your conductor instances point to the same MongoDB and Redis instance, and they will operate as a load balanced fleet.

## Environment Variables to configure

You can configure the database and Redis backplane by setting environment variables.

```
dbhost: <<insert connection string to your MongoDB server>>
redis: <<insert connection string to your Redis server>> (optional)
```

If you would like to setup a conductor container (API on port 5001) and a MongoDB container at the same time and have them linked, use this docker compose file:

```Dockerfile
version: '3'
services:
  conductor:
    image: danielgerlag/conductor
    ports:
    - "5001:80"
    links:
    - mongo
    environment:
      dbhost: mongodb://mongo:27017/
  mongo:
    image: mongo
```

# Getting started

## Creating your first workflow

We'll start by defining a simple workflow that will log "Hello world" as it's first step and then "Goodbye!!!" as it's second and final step. We `POST` the definition to `api/definition` in either `YAML` or `JSON`.

```http
POST /api/definition
Content-Type: application/yaml
```

```yml
Id: Hello1
Steps:
  - Id: Step1
    StepType: EmitLog
    NextStepId: Step2
    Inputs:
      Message: '"Hello world"'
      Level: '"Information"'
  - Id: Step2
    StepType: EmitLog
    Inputs:
      Message: '"Goodbye!!!"'
      Level: '"Information"'
```

or

```http
POST /api/definition
Content-Type: application/json
```

```json
{
  "Id": "Hello1",
  "Steps": [
    {
      "Id": "Step1",
      "StepType": "EmitLog",
      "NextStepId": "Step2",
      "Inputs": {
        "Message": "\"Hello world\"",
        "Level": "\"Information\""
      }
    },
    {
      "Id": "Step2",
      "StepType": "EmitLog",
      "Inputs": {
        "Message": "\"Goodbye!!!\"",
        "Level": "\"Information\""
      }
    }
  ]
}
```

Now, lets test it by invoking a new instance of our workflow.
We do this with a `POST` to `/api/workflow/Hello1`

```
POST /api/workflow/Hello1
Content-Type: application/json
```

```json
{}
```

We can also rewrite our workflow to pass custom data to any input on any of it's steps.

```yml
Id: Hello2
Steps:
  - Id: Step1
    StepType: EmitLog
    Inputs:
      Message: data.CustomMessage
      Level: '"Information"'
```

Now, when we start a new instance of the workflow, we also initialize it with some data.

```
POST /api/workflow/Hello2
Content-Type: application/json
```

```json
{
  "CustomMessage": "foobar"
}
```

or

```
POST /api/workflow/Hello2
Content-Type: application/x-yaml
```

```yaml
CustomMessage: foobar
```

# Resources

- Download the [Postman Collection](https://raw.githubusercontent.com/danielgerlag/conductor/master/docs/Conductor.postman_collection.json)
