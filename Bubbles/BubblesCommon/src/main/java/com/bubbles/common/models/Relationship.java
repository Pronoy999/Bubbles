package com.bubbles.common.models;

import lombok.Data;

@Data
public class Relationship {
    private String id;
    private Node leftNode;
    private Node rightNode;
    private Type type;
    private Object data;
}
