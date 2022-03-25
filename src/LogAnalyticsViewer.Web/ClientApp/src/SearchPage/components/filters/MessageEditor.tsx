import { Classes, Icon } from "@blueprintjs/core";
import { CROSS } from "@blueprintjs/icons/lib/esm/generated/iconNames";
import classNames from "classnames";
import React, { useEffect, useRef } from "react";
import { filterActions } from "../../state/filtersReducer";
import { MessageFilter } from "../../types";
import styles from "./Filters.module.css";

type MessageEditorProps = {
    message: MessageFilter;
    actions: typeof filterActions;
};
export const MessageEditor: React.FC<MessageEditorProps> = ({ message, actions }) => {
    const onChange = (e: React.ChangeEvent<HTMLInputElement>) => actions.updateMessageFilter(message.id, e.target.value);
    const ref = useRef<HTMLInputElement>();
    useEffect(() => {
        ref.current.focus();
    }, []);

    const isLike = message.type === "like";
    const label = message.type === "like" ? "Message contains" : "Message not contains";
    const inputClassName = classNames(Classes.INPUT, isLike ? Classes.INTENT_SUCCESS : Classes.INTENT_DANGER);
    const wrapperClassName = classNames(styles.message_editor, isLike ? styles.message_editor_like : styles.message_editor_not_like);

    return (
        <div className={wrapperClassName}>
            <div className={styles.message_editor_label}>{label}</div>
            <input
                ref={ref}
                value={message.text}
                onChange={onChange}
                className={inputClassName}
            />
            <Icon
                icon={CROSS}
                className={styles.message_editor_icon}
                onClick={() => actions.deleteMessageFilter(message.id)}
            />
        </div>
    );
};