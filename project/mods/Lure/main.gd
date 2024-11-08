extends Node

signal mod_loaded(mod) # mod: LureMod
signal mods_loaded

const LureMod := preload("res://mods/Lure/classes/lure_mod.gd")
const Loader := preload("res://mods/Lure/modules/loader.gd")

var mods: Dictionary setget _set_nullifier


func _init() -> void:
	pass


func _enter_tree() -> void:
	pass


func _ready() -> void:
	pass


# Returns a mod matching the given mod ID
func get_mod(mod_id: String) -> LureMod:
	return mods.get(mod_id)


# Register a mod with Lure
# Do not call this if you don't know what you're doing: Mod registry is automatic.
func _register_mod(mod: LureMod) -> void:
	if not mod is LureMod:
		return
	
	if not mod in mods:
		mods[mod.mod_id] = mod


# Prevents other mods from modifying variables
func _set_nullifier(value) -> void:
	return