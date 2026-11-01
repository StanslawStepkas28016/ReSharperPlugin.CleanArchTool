package com.jetbrains.rider.plugins.cleanarchtool.actions
    
import com.jetbrains.rd.ui.bedsl.dsl.description
import com.jetbrains.rider.actions.RiderActionsBundle
import com.jetbrains.rider.actions.base.RiderAnAction
import icons.CleanArchToolThemedIcons
import icons.ReSharperIcons

class ManagePluginSettingsAction : RiderAnAction(
    backendActionId = "ManagePluginSettings", 
    icon = CleanArchToolThemedIcons.Icon
)