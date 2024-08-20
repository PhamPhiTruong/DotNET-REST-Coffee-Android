package com.nlu.packages.dotnet_callapi.enumtype;

import java.util.HashMap;
import java.util.Map;

public enum EProductType {
    DRINK("DRINK"), FOOD("FOOD"), FRUIT("FRUIT");
    private final String text;
    private EProductType(String text) {
        this.text = text;
    }
    public static final Map<String, com.nlu.packages.dotnet_callapi.enumtype.EProductType> MAP_TO_PATHNAME;
    static {
        MAP_TO_PATHNAME = new HashMap<>();
        MAP_TO_PATHNAME.put("DRINK", DRINK);
        MAP_TO_PATHNAME.put("FOOD", FOOD);
        MAP_TO_PATHNAME.put("FRUIT", FRUIT);
    };
    @Override
    public String toString() {
        return text;
    }

    public com.nlu.packages.dotnet_callapi.enumtype.EProductType valueOfLabel(String label) {
        for (com.nlu.packages.dotnet_callapi.enumtype.EProductType e : values()) {
            if (e.text.equals(label)) {
                return e;
            }
        }
        return null;
    }
}
