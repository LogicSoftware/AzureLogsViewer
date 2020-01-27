import { Button, Menu, MenuItem, Popover, Position } from "@blueprintjs/core";
import { CARET_DOWN } from "@blueprintjs/icons/lib/esm/generated/iconNames";
import React from "react";
import { MessageFilterType } from "../../types";

type AddMessageFilterProps = {
    add: (type: MessageFilterType) => any;
}
export const AddMessageFilter: React.FC<AddMessageFilterProps> = ({ add }) => {
    return (
        <Popover position={Position.BOTTOM_LEFT}>
            <Button
                rightIcon={CARET_DOWN}
                text={"Add message filter"}
            />
            <Menu>
                <MenuItem
                    text={"Contains"}
                    onClick={() => add("like")}
                />
                <MenuItem
                    text={"Not Contains"}
                    onClick={() => add("notlike")}
                />
            </Menu>
        </Popover>
    )
};