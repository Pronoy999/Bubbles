package com.bubbles.common.models;

import lombok.Data;

import java.util.List;

@Data
public class Node {
    private String id;
    private Type type;
    private List<Relationship> relationships;
    private Object data;
}
