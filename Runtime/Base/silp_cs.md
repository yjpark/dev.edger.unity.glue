# PARAM_DECLARE(index) #
```
public readonly string Param${index};
public readonly bool IsState${index};
public readonly TP${index} Value${index};
```

# PARAM_INIT(index) #
```
InitParam(param${index}, out Param${index}, out IsState${index}, out Value${index});
```

# PARAM_VALUE(index) #
```
TP${index} value${index} = Value${index};
if (IsState${index}) {
    value${index} = Param.GetStateValue<TP${index}>(runtime, caret, this, Param${index});
}
```
