# Using custom steps in your workflow

Conductor also allows you to define your own steps that can be used within your workflows. Currently, the only supported language is Python. More languages will be implemented in the future.

The following call creates a step called `add`, which is a Python script that sets c to a + b

```
POST /api/step/add
Content-Type: text/x-python
```

```python
c = a + b
```

Next, we create a workflow definiton that invokes our `add` step with values from the internal data object of the workflow and outputs the result to a log.

```http
POST /api/definition
Content-Type: application/yaml
```

```yml
Id: MyStepWorkflow
Steps:
  - Id: Step1
    StepType: add
    Inputs:
      a: data.Value1
      b: data.Value2
    NextStepId: Step2
    Outputs:
      Result: step["c"]
  - Id: Step2
    StepType: EmitLog
    Inputs:
      Message: '"Answer is " + str(data.Result)'
```

Now, lets test it by invoking a new instance of our workflow

```
POST /api/workflow/MyStepWorkflow
Content-Type: application/json
```

```json
{
  "Value1": 7,
  "Value2": 3
}
```

Response:

```json
{
  "workflowId": "5d0ab0ff23576b61e4afbcfb",
  "data": {
    "Value1": 7,
    "Value2": 3
  },
  "definitionId": "MyStepWorkflow",
  "version": 1,
  "status": "Runnable"
}
```

This should output the follow to the logs

```
Answer is 10
```

Also, you can inspect the internal data of the workflow

```
GET /api/workflow/5d0ab0ff23576b61e4afbcfb
```

Response:

```json
{
  "workflowId": "5d0ab0ff23576b61e4afbcfb",
  "data": {
    "Value1": 7,
    "Value2": 3,
    "Result": 10
  },
  "definitionId": "MyStepWorkflow",
  "version": 1,
  "status": "Complete"
}
```
