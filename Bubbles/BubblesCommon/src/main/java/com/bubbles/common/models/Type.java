package com.bubbles.common.models;

import lombok.Data;

import java.util.List;

@Data
public class Type {
    private List<String> nodeIds;
    private String typeName;
}
